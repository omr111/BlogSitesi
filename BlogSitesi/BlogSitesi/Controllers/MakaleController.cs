using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSitesi.App_Classes;
using System.IO;
using System.Drawing;
using System.Web.Helpers;
using System.Web.UI;
using System.Xml.Schema;
using BlogSitesi.myModels;

namespace BlogSitesi.Controllers
{
    
    public class MakaleController : Controller
    {
        u9139968_blogContext ctx = new u9139968_blogContext();
        //
        // GET: /Makale/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult TariheGoreListe(int yil,int ay)
        {
            if (yil == null || ay == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.yil = yil;
                ViewBag.ay = ay;

                return View();
            }
           
        }
        [AllowAnonymous]
        public ActionResult MakaleListele(int yil, int ay,int? page)
        {

           

            int pageIndex;
            int pagingCount = 4;
            List<Makale> sendMakale = null;
            if (!page.HasValue)
            {
                sendMakale = ctx.Makales.Where(x => x.YayinTarihi.Year == yil && x.YayinTarihi.Month == ay).OrderByDescending(x => x.YayinTarihi).Take(pagingCount).ToList();
              
            }
            else
            {
                pageIndex = pagingCount * page.Value;
                sendMakale = ctx.Makales.Where(x => x.YayinTarihi.Year == yil && x.YayinTarihi.Month == ay).OrderByDescending(x => x.YayinTarihi).Skip(pageIndex).Take(pagingCount).ToList();
             
            }

            if (Request.IsAjaxRequest())
            {
              
                return PartialView("_MakaleListele", sendMakale);
              
                
            }
            return View("_MakaleListele", sendMakale);
           
        }
        [AllowAnonymous]
        public ActionResult Detay(int id)
        {  
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.kullanici = Session["Kullanici"];
                var data = ctx.Makales.FirstOrDefault(x=>x.id==id);
                data.Goruntulenme++;
                ctx.SaveChanges();
                return View(data); 
            }
          
           
        }
        [Authorize]
        [HttpPost]
        public ActionResult YorumYaz(Yorum yorum)
        {
            if (ModelState.IsValid)
            {
                yorum.EklemeTarihi = DateTime.Now;
                yorum.Aktif = true;
                yorum.Baslik = "";
                ctx.Yorums.Add(yorum);
                ctx.SaveChanges();
                return RedirectToAction("Detay", new { id = yorum.MakaleID });

            }
            else
            {

                return RedirectToAction("Detay", new {id = yorum.MakaleID});
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Begen(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var makale = ctx.Makales.FirstOrDefault(x => x.id == id);
                if (makale!=null)
                {
                    var kullaniciName = User.Identity.Name;
                    var kID = ctx.Kullanicis.FirstOrDefault(x => x.Nick == kullaniciName);
                    var varMi = ctx.KullaniciBegenis.FirstOrDefault(x => x.Makale.id == makale.id && x.KullaniciID == kID.id);
                    if (varMi == null)
                    {
                        KullaniciBegeni begen = new KullaniciBegeni();
                        begen.KullaniciID = kID.id;
                        begen.MakaleID = makale.id;
                        makale.KullaniciBegenis.Add(begen);
                        ctx.SaveChanges();
                        return Json(new { id = 1, count = makale.KullaniciBegenis.Count });
                    }
                    else
                    {
                        ctx.KullaniciBegenis.Remove(varMi);
                        ctx.SaveChanges();
                        return Json(new { id = 2, count = makale.KullaniciBegenis.Count });
                    }
                    
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("login", "Kullanici");
            }
         

          
          
           
        }


        [Authorize(Roles = "Admin,Moderator,Yazar")]
       
        [ValidateInput(false)]
        public ActionResult MakaleYaz()
        {
            ViewBag.kategori = ctx.Kategoris.ToList();
            return View();

        }
        [Authorize(Roles = "Admin,Moderator,Yazar")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult MakaleYaz(Makale m,HttpPostedFileBase Resim,string etiketler)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (m != null) { 
                        Kullanici aktif = Session["Kullanici"] as Kullanici;
                        m.YayinTarihi = DateTime.Now;
                        m.MakaleTipID = 1;
                        if(aktif!=null)
                            m.YazarID = aktif.id;
                        if (Resim!=null)
                        {
                            resimKaydet kaydet=ResimKaydet(Resim,HttpContext);
                            m.BuyukResimYol= kaydet.buyukResimYol;
                            m.kucukResimYol=  kaydet.kucukResimYol;
                            m.resimAlt = kaydet.resimAltText;
                        }
                        else
                        {
                            ViewData["resim"] = "Lütfen Bir Resim Ekleyiniz";
                            ViewBag.kategori = ctx.Kategoris.ToList();
                            return View("MakaleYaz");
                        }
           
          
                        ctx.Makales.Add(m);
                        ctx.SaveChanges();

                        string[] eti = etiketler.Split(',');
                        foreach (string etiket in eti)
                        {
                            Etiket etk = ctx.Etikets.FirstOrDefault(x => x.Adi.ToLower() == etiket.ToLower().Trim());
                            MakaleEtiket makaleyeEtiketEkle = new MakaleEtiket();
                            if (etk == null)
                            {
                                etk = new Etiket();
                                etk.Adi = etiket;
                                ctx.Entry(etk).State = EntityState.Added;
                                ctx.SaveChanges();

                                makaleyeEtiketEkle.EtiketID = etk.id;
                                makaleyeEtiketEkle.MakaleID = m.id;
                                ctx.MakaleEtikets.Add(makaleyeEtiketEkle);
                                ctx.SaveChanges();
                            }

                            MakaleEtiket makaleEtiket = ctx.MakaleEtikets.FirstOrDefault(x => x.Etiket.Adi == etiket.ToLower() && x.MakaleID == m.id);
                            if (makaleEtiket == null)
                            {

                                makaleyeEtiketEkle.EtiketID = etk.id;
                                makaleyeEtiketEkle.MakaleID = m.id;
                                ctx.MakaleEtikets.Add(makaleyeEtiketEkle);
                                ctx.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.kategori = ctx.Kategoris.ToList();
                return View("MakaleYaz");
            }
           
        }

        [Authorize(Roles = "Admin,Moderator,Yazar")]
   
        
        [ValidateInput(false)]
        public ActionResult MakaleDuzenle(int id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                u9139968_blogContext ctx = new u9139968_blogContext();
                //List<MakaleEtiket> makaleEtiketleri=ctx.MakaleEtikets.Where(x => x.MakaleID == id).ToList();
                Makale makale = ctx.Makales.FirstOrDefault(x => x.id == id);
                ViewBag.kategori = ctx.Kategoris.ToList();
                string etiketAdi = "";
                if (makale.MakaleEtikets.Count > 0)
                {
                    foreach (var etiket in makale.MakaleEtikets)
                    {

                        etiketAdi += etiket.Etiket.Adi + ",";
                    }
                }



                int count = etiketAdi.Length - 1;
                var virgulSil = etiketAdi.Remove((int)count, 1);
                ViewBag.etiketler = virgulSil;
                return View("MakaleDuzenle", makale);
            }
        }
        [Authorize(Roles = "Admin,Moderator,Yazar")]
       
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MakaleDuzenle(Makale makale,string etiketler,HttpPostedFileBase Resim  )
        {
            if (ModelState.IsValid)
            {
                try
                {

                    Makale makaleUpdated = ctx.Makales.FirstOrDefault(x => x.id == makale.id);
                    if (Resim != null)
                    {


                      
                        if (System.IO.File.Exists(Server.MapPath(makaleUpdated.BuyukResimYol)))
                        {
                            System.IO.File.Delete(Server.MapPath(makaleUpdated.BuyukResimYol));

                        }
                        if (System.IO.File.Exists(Server.MapPath(makaleUpdated.kucukResimYol)))
                        {
                            System.IO.File.Delete(Server.MapPath(makaleUpdated.kucukResimYol));
                        }


                        resimKaydet kaydet = ResimKaydet(Resim, HttpContext);
                        makaleUpdated.BuyukResimYol = kaydet.buyukResimYol;
                        makaleUpdated.kucukResimYol = kaydet.kucukResimYol;
                        makaleUpdated.resimAlt = kaydet.resimAltText;
                    }
                    makaleUpdated.Baslik = makale.Baslik;

                    makaleUpdated.icerik = makale.icerik;

                    ctx.Entry(makaleUpdated).State = EntityState.Modified;
                    //todo hata kontrolü yapılacak.
                    ctx.SaveChanges();
                    string[] tags = etiketler.Split(',');
                    List<MakaleEtiket> makaleEtikets = ctx.MakaleEtikets.Where(x => x.MakaleID == makaleUpdated.id).ToList();
                    foreach (var etiket in makaleEtikets)
                    {
                        ctx.Entry(etiket).State = EntityState.Deleted;
                    }

                    ctx.SaveChanges();
                    foreach (var tag in tags)
                    {
                        Etiket searchEtiket = ctx.Etikets.FirstOrDefault(x => x.Adi == tag.ToLower());
                        MakaleEtiket makaleyeEtiketEkle = new MakaleEtiket();
                        if (tag != "" && searchEtiket == null)
                        {
                            Etiket etiketEkle = new Etiket();
                            etiketEkle.Adi = tag.ToLower();
                            ctx.Etikets.Add(etiketEkle);
                            ctx.SaveChanges();

                            makaleyeEtiketEkle.EtiketID = etiketEkle.id;
                            makaleyeEtiketEkle.MakaleID = makaleUpdated.id;
                            ctx.MakaleEtikets.Add(makaleyeEtiketEkle);
                            ctx.SaveChanges();
                        }

                        MakaleEtiket makaleEtiket = ctx.MakaleEtikets.FirstOrDefault(x => x.Etiket.Adi == tag.ToLower() && x.MakaleID == makaleUpdated.id);
                        if (makaleEtiket == null)
                        {

                            makaleyeEtiketEkle.EtiketID = searchEtiket.id;
                            makaleyeEtiketEkle.MakaleID = makaleUpdated.id;
                            ctx.MakaleEtikets.Add(makaleyeEtiketEkle);
                            ctx.SaveChanges();
                        }



                    }

                    //return Json(new { message = "Başarılı" }, JsonRequestBehavior.AllowGet);
                    return RedirectToAction("Detay", new { id = makale.id });
                    //return Json("Başarılı", JsonRequestBehavior.AllowGet);

                    //return Json(new { message = "Başarılı" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {

                    
                    Session["errorMessage"] += e.Message;
                    return RedirectToAction("MakaleDuzenle");
                }
            }
            else
            {
                ViewBag.etiketler = etiketler;
                ViewBag.kategori=ctx.Kategoris.ToList();
                return View("MakaleDuzenle", makale);
            }
        }

        [Authorize]
       public static resimKaydet ResimKaydet(HttpPostedFileBase Resim,HttpContextBase ctx)
        {
            u9139968_blogContext context = new u9139968_blogContext();
            
            int kucukEn = Setttings.KucukResimYol.Width;
            int kucukBoy = Setttings.KucukResimYol.Height;
        
            int buyukEn = Setttings.BuyukResimYol.Width;
            int buyuBoy = Setttings.BuyukResimYol.Height;
            string newName = "";
            if (Resim.FileName.Length>10)
            {
                newName = Path.GetFileNameWithoutExtension(Resim.FileName.Substring(0, 10)) + "-" + Guid.NewGuid() + Path.GetExtension(Resim.FileName);
            }
            else
            {
                 newName = Path.GetFileNameWithoutExtension(Resim.FileName) + "-" + Guid.NewGuid() + Path.GetExtension(Resim.FileName);
            }
            Image orjResim = Image.FromStream(Resim.InputStream);
         
            Bitmap kucukResim = new Bitmap(orjResim, kucukEn, kucukBoy);
            Bitmap buyukResim = new Bitmap(orjResim, buyukEn, buyuBoy);
            
            kucukResim.Save(ctx.Server.MapPath("~/Content/KucukResim/"+newName));
        
            buyukResim.Save(ctx.Server.MapPath("~/Content/BuyukResim/" + newName));
          
         

         resimKaydet kaydet =new resimKaydet();
         kaydet.buyukResimYol="/Content/BuyukResim/" + newName;
         kaydet.kucukResimYol = "/Content/KucukResim/" + newName;
         if (Resim.FileName.Length>50)
         {
             kaydet.resimAltText = Resim.FileName.Substring(0,49);
         }
         else
         {
             kaydet.resimAltText = Resim.FileName;
         }
          
           return kaydet;
        }

       [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
       public JsonResult MakaleSil(int id)
       {
           if (id == null)
           {
               return Json(new { isOK = 0, message = "Silinmek İstenilen Makale Bulunamadı!" });
           }
           else
           {
               try
               {
                   Makale makale = ctx.Makales.FirstOrDefault(x => x.id == id);

                   if (makale != null)
                   {
                       if (System.IO.File.Exists(Server.MapPath(makale.BuyukResimYol)))
                       {
                           System.IO.File.Delete(Server.MapPath(makale.BuyukResimYol));

                       }
                       if (System.IO.File.Exists(Server.MapPath(makale.kucukResimYol)))
                       {
                           System.IO.File.Delete(Server.MapPath(makale.kucukResimYol));
                       }
                   }



                   List<MakaleEtiket> etikets = ctx.MakaleEtikets.Where(x => x.MakaleID == id).ToList();
                   if (etikets.Count > 0)
                   {
                       foreach (var etiket in etikets)
                       {
                           ctx.Entry(etiket).State = EntityState.Deleted;
                       }

                       ctx.SaveChanges();
                   }

                   List<Yorum> yorums = ctx.Yorums.Where(x => x.MakaleID == id).ToList();
                   if (yorums.Count > 0)
                   {
                       foreach (var yorum in yorums)
                       {
                           ctx.Entry(yorum).State = EntityState.Deleted;
                       }

                       ctx.SaveChanges();
                   }

                   List<KullaniciBegeni> begenis = ctx.KullaniciBegenis.Where(x => x.MakaleID == id).ToList();

                   if (begenis.Count > 0)
                   {
                       foreach (var kullanici in begenis)
                       {
                           ctx.Entry(begenis).State = EntityState.Deleted;
                       }

                       ctx.SaveChanges();
                   }

                   ctx.Entry(makale).State = EntityState.Deleted;
                   int result = ctx.SaveChanges();
                   if (result > 0)
                   {
                       return Json(new { isOK = 1, message = "Silme İşlemi Başarılı" });
                   }
                   else
                   {
                       return Json(new { isOK = 0, message = "Silme İşlemi Sırasında Bir Hata Meydana Geldi !" });
                   }


               }

               catch (Exception e)
               {
                   return Json(new { isOK = 0, message = e.Message });
               }
           }
       }
	}
}