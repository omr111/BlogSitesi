using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSitesi.App_Classes;
using System.IO;
using System.Drawing;

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
        public string Begen(int id)
        {
            var makale = ctx.Makales.FirstOrDefault(x => x.id == id);

            var kullaniciName =User.Identity.Name;
              
            var kID = ctx.Kullanicis.FirstOrDefault(x => x.Nick == kullaniciName);
        
            
            
          // var varMi = ctx.KullaniciBegenis.FirstOrDefault(x => x.Makale.id == id && x.KullaniciID == kID.id);
               //if (varMi == null)
               //{
           makale.Kullanicis.Add(kID);
                   makale.Begeni++;
               //}
               //else
               //    makale.Begeni--;
            ctx.SaveChanges();
            return makale.Begeni.ToString();
        }
        public ActionResult MakaleYaz()
        {
            ViewBag.kategori = ctx.Kategoris.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult MakaleYaz(Makale m,HttpPostedFileBase Resim,string etiketler)
        {
            if (m != null) { 
            Kullanici aktif = Session["Kullanici"] as Kullanici;
            m.YayinTarihi = DateTime.Now;
            m.MakaleTipID = 1;
                if(aktif!=null)
            m.YazarID = aktif.id;
            m.KapakResimID = ResimKaydet(Resim,HttpContext);
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
                    m.Etikets.Add(etk);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Index","Home");
        }

       public static int ResimKaydet(HttpPostedFileBase Resim,HttpContextBase ctx)
        {
            BlogContext context = new BlogContext();
            
            int kucukEn = Setttings.KucukResimYol.Width;
            int kucukBoy = Setttings.KucukResimYol.Height;
            int ortaEn = Setttings.OrtaResimYol.Width;
            int ortaBoy = Setttings.OrtaResimYol.Height;
            int buyukEn = Setttings.BuyukResimYol.Width;
            int buyuBoy = Setttings.BuyukResimYol.Height;
            string newName = Path.GetFileNameWithoutExtension(Resim.FileName)+"-"+Guid.NewGuid()+ Path.GetExtension(Resim.FileName);
            Image orjResim = Image.FromStream(Resim.InputStream);
         
            Bitmap kucukResim = new Bitmap(orjResim, kucukEn, kucukBoy);
            Bitmap ortaResim = new Bitmap(orjResim, ortaEn, ortaBoy);
            Bitmap buyukResim = new Bitmap(orjResim, buyukEn, buyuBoy);
            kucukResim.Save(ctx.Server.MapPath("~/Content/KucukResim/"+newName));
            ortaResim.Save(ctx.Server.MapPath("~/Content/OrtaResim/" + newName));
            buyukResim.Save(ctx.Server.MapPath("~/Content/BuyukResim/" + newName));
           Kullanici k = (Kullanici)ctx.Session["Kullanici"];
          Models.Resim dbResim= new Models.Resim();
           dbResim.Adi=Resim.FileName;
           dbResim.KucukResimYol="/Content/KucukResim/"+newName;
           
          
           dbResim.OrtaResimYol="/Content/OrtaResim/" + newName;
           dbResim.BuyukResimYol = "/Content/BuyukResim/" + newName;
           dbResim.EklemeTarihi = DateTime.Now;
           dbResim.EkleyenID = k.id;
           context.Resims.Add(dbResim);
           context.SaveChanges();
           return dbResim.id;
        }
	}
}