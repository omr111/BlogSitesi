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

namespace BlogSitesi.Controllers
{
    
    public class MakaleController : Controller
    {
        BlogContext ctx = new BlogContext();
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
            ViewBag.yil = yil;
            ViewBag.ay = ay;
      
            return View();
        }
        [AllowAnonymous]
        public ActionResult MakaleListele(int yil, int ay)
        {
            var data = ctx.Makales.Where(x => x.YayinTarihi.Year == yil && x.YayinTarihi.Month == ay);
            
            return View("_MakaleListele", data);
        }
        [AllowAnonymous]
        public ActionResult Detay(int id)
        {  
            ViewBag.kullanici = Session["Kullanici"];
            var data = ctx.Makales.FirstOrDefault(x=>x.id==id);
            data.Goruntulenme++;
            ctx.SaveChanges();
         

            return View(data);
        }
        public ActionResult YorumYaz(Yorum yorum)
        {
            yorum.EklemeTarihi = DateTime.Now;
            yorum.Aktif = false;
            yorum.Baslik = "";
            ctx.Yorums.Add(yorum);
            ctx.SaveChanges();
            return RedirectToAction("Detay", new { id = yorum.MakaleID });

        }
        //public string Begen(int id)
        //{
        //  //  var makale = ctx.Makales.FirstOrDefault(x => x.id == id);

        //  //  var kullaniciName =User.Identity.Name;
              
        //  //  var kID = ctx.Kullanicis.FirstOrDefault(x => x.Nick == kullaniciName);
        
            
            
        //  //// var varMi = ctx.KullaniciBegenis.FirstOrDefault(x => x.Makale.id == id && x.KullaniciID == kID.id);
        //  //     //if (varMi == null)
        //  //     //{
        //  // makale.Kullanicis.Add(kID);
        //  //         makale.Begeni++;
        //  //     //}
        //  //     //else
        //  //     //    makale.Begeni--;
        //  //  ctx.SaveChanges();
        //    //return makale.Begeni.ToString();
        //}
       
        [ValidateInput(false)]
        public ActionResult MakaleYaz()
        {
            ViewBag.kategori = ctx.Kategoris.ToList();
            return View();

        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult MakaleYaz(Makale m,HttpPostedFileBase Resim,string etiketler)
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
                        m.KapakResimID = ResimKaydet(Resim,HttpContext);
                    }
                    else
                    {
                        return View(Json("Lütfen Bir Resim Ekleyiniz", JsonRequestBehavior.AllowGet));
                    }
           
          
                    ctx.Makales.Add(m);
                    ctx.SaveChanges();

                    string[] eti = etiketler.Split(',');
                    foreach(string etiket in eti)
                    {
                        Etiket etk = ctx.Etikets.FirstOrDefault(x => x.Adi.ToLower() == etiket.ToLower().Trim());
                        if (etk==null)
                        {
                            etk = new Etiket();
                            etk.Adi = etiket;
                            
                            ctx.SaveChanges();
                        }
                        MakaleEtiket makaleEtiket=new MakaleEtiket();
                        makaleEtiket.MakaleID = m.id;
                        makaleEtiket.EtiketID = etk.id;
                        m.MakaleEtikets.Add(makaleEtiket);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("Index","Home");
        }
       
        [ValidateInput(false)]
        public ActionResult MakaleDuzenle(int id)
        {
            BlogContext ctx =new BlogContext();
            //List<MakaleEtiket> makaleEtiketleri=ctx.MakaleEtikets.Where(x => x.MakaleID == id).ToList();
            Makale makale = ctx.Makales.FirstOrDefault(x => x.id == id);
            ViewBag.kategori=ctx.Kategoris.ToList();
            string etiketAdi = "";
            if (makale.MakaleEtikets.Count>0)
            {
                foreach (var etiket in makale.MakaleEtikets)
                    {
                        
                        etiketAdi += etiket.Etiket.Adi + ",";
                    }
            }
            

            string kapakResimUrl = ctx.Resims.FirstOrDefault(x => x.id ==makale.KapakResimID ).BuyukResimYol;
            if (!string.IsNullOrEmpty(kapakResimUrl))
            {
                ViewBag.kapakResim = kapakResimUrl;
            }
            else
            {
                ViewBag.kapakResim = '#';
            }

            var devide=kapakResimUrl.Split('/');
            var fileName = devide[devide.Length - 1];
            
           
            int count = etiketAdi.Length - 1;
           var virgulSil=etiketAdi.Remove((int)count, 1);
           ViewBag.etiketler = virgulSil;
            return View("MakaleDuzenle", makale);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult MakaleDuzenle(Makale makale,string etiketler,HttpPostedFileBase Resim  )
        {
            try
            {
               
                Makale m = ctx.Makales.FirstOrDefault(x => x.id == makale.id);
                if (Resim != null)
                {
                    
                    Resim makaleResim = ctx.Resims.FirstOrDefault(x => x.id == m.KapakResimID);
                   
                        if (System.IO.File.Exists(Server.MapPath("/Content/BuyukResim/" + makaleResim.BuyukResimYol)))
                        {
                            System.IO.File.Delete(Server.MapPath("/Content/BuyukResim/" + makaleResim.BuyukResimYol));

                        }
                        if (System.IO.File.Exists(Server.MapPath("/Content/BuyukResim/" + makaleResim.KucukResimYol)))
                        {
                            System.IO.File.Delete(Server.MapPath("/Content/BuyukResim/" + makaleResim.BuyukResimYol));
                        }

                        ctx.Entry(makaleResim).State = EntityState.Deleted;
                        ctx.SaveChanges();
                    m.KapakResimID = ResimKaydet(Resim, HttpContext);
                }
                m.Baslik = makale.Baslik;
                
                m.icerik = makale.icerik;
                ctx.Entry(m).State = EntityState.Modified;
                //todo hata kontrolü yapılacak.
                ctx.SaveChanges();
                string[] tags = etiketler.Split(',');
                List < MakaleEtiket > makaleEtikets = ctx.MakaleEtikets.Where(x => x.MakaleID == m.id).ToList();
                foreach (var etiket  in makaleEtikets)
                {
                    ctx.Entry(etiket).State = EntityState.Deleted;
                }

                ctx.SaveChanges();
                foreach (var tag in tags)
                {
                    Etiket searchEtiket = ctx.Etikets.FirstOrDefault(x => x.Adi == tag);
                    if (tag != "" && searchEtiket == null)
                    {
                        Etiket etiketEkle=new Etiket();
                        etiketEkle.Adi = tag.ToLower();
                        ctx.Etikets.Add(etiketEkle);
                        ctx.SaveChanges();

                    }

                    MakaleEtiket makaleEtiket = ctx.MakaleEtikets.FirstOrDefault(x => x.Etiket.Adi == tag.ToLower());
                   
                        MakaleEtiket makaleyeEtiketEkle = new MakaleEtiket();
                        makaleyeEtiketEkle.EtiketID = searchEtiket.id;
                        makaleyeEtiketEkle.MakaleID = m.id;
                        ctx.MakaleEtikets.Add(makaleyeEtiketEkle);
                        ctx.SaveChanges();
                    
                  
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
       public static int ResimKaydet(HttpPostedFileBase Resim,HttpContextBase ctx)
        {
            BlogContext context = new BlogContext();
            
            int kucukEn = Setttings.KucukResimYol.Width;
            int kucukBoy = Setttings.KucukResimYol.Height;
        
            int buyukEn = Setttings.BuyukResimYol.Width;
            int buyuBoy = Setttings.BuyukResimYol.Height;
            string newName = Path.GetFileNameWithoutExtension(Resim.FileName)+"-"+Guid.NewGuid()+ Path.GetExtension(Resim.FileName);
            Image orjResim = Image.FromStream(Resim.InputStream);
         
            Bitmap kucukResim = new Bitmap(orjResim, kucukEn, kucukBoy);
            Bitmap buyukResim = new Bitmap(orjResim, buyukEn, buyuBoy);
            
            kucukResim.Save(ctx.Server.MapPath("~/Content/KucukResim/"+newName));
        
            buyukResim.Save(ctx.Server.MapPath("~/Content/BuyukResim/" + newName));
           Kullanici k = (Kullanici)ctx.Session["Kullanici"];
          Resim dbResim= new Models.Resim();
          //resmin alt'ı olarak kullanacağım filename'i
           dbResim.Adi=Resim.FileName;

           dbResim.KucukResimYol="/Content/KucukResim/"+newName;
           dbResim.BuyukResimYol = "/Content/BuyukResim/" + newName;

           //dbResim.EklemeTarihi = Convert.ToDateTime(DateTime.Now.ToString("d"));
           dbResim.EkleyenID = k.id;
           context.Resims.Add(dbResim);
           
           context.SaveChanges();
           return dbResim.id;
        }
        [HttpPost]
       public JsonResult MakaleSil(int id)
       {
           try
           {
               
                   Resim makaleResim = ctx.Resims.FirstOrDefault(x => x.id == id);

                   if (makaleResim!=null)
                   {
                       if (System.IO.File.Exists(Server.MapPath("/Content/BuyukResim/" + makaleResim.BuyukResimYol)))
                       {
                           System.IO.File.Delete(Server.MapPath("/Content/BuyukResim/" + makaleResim.BuyukResimYol));
                   
                       }
                       if (System.IO.File.Exists(Server.MapPath("/Content/BuyukResim/" + makaleResim.KucukResimYol)))
                       {
                           System.IO.File.Delete(Server.MapPath("/Content/BuyukResim/" + makaleResim.BuyukResimYol));
                       }
                   }

                   if (makaleResim!=null)
                   {
                        ctx.Entry(makaleResim).State = EntityState.Deleted;
                        ctx.SaveChanges();
                   }

                  List<MakaleEtiket> etikets= ctx.MakaleEtikets.Where(x => x.MakaleID==id).ToList();
                  if (etikets.Count>0)
                  {
                      foreach (var etiket in etikets)
                      {
                          ctx.Entry(etiket).State = EntityState.Deleted;
                      }

                      ctx.SaveChanges();
                  }

                  List<Yorum> yorums = ctx.Yorums.Where(x => x.MakaleID == id).ToList();
                  if (yorums.Count>0)
                  {
                      foreach (var yorum in yorums)
                      {
                          ctx.Entry(yorum).State = EntityState.Deleted;
                      }

                      ctx.SaveChanges();
                  }

                  List<KullaniciBegeni> begenis = ctx.KullaniciBegenis.Where(x => x.MakaleID == id).ToList();

                  if (begenis.Count>0  )
                  {
                      foreach (var kullanici in begenis)
                      {
                          ctx.Entry(begenis).State = EntityState.Deleted;
                      }

                      ctx.SaveChanges();
                  }
                   Makale makale = ctx.Makales.FirstOrDefault(x => x.id == id);
                   ctx.Entry(makale).State = EntityState.Deleted;
                   int result = ctx.SaveChanges();
                   if (result>0)
                   {
                       return Json(new {isOK = 1, message = "Silme İşlemi Başarılı"});
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