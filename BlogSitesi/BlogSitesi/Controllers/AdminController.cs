using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BlogSitesi.Controllers
{
    
    public class AdminController : Controller
    {
     
        BlogContext ctx = new BlogContext();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciListesi()
        {
            List<Kullanici> kullanici = ctx.Kullanicis.Where(x=>x.YazarMi==false).ToList();
            return View(kullanici);

        }
        public ActionResult YazarListesi()
        {
            List<Kullanici> yazarlar = ctx.Kullanicis.Where(x => x.YazarMi == true).ToList();
 
            return View(yazarlar);
        }
        public ActionResult TumMakale()
        {
            List<Makale> makaleler = ctx.Makales.ToList();
            return View(makaleler);
        }
        public ActionResult yazarIstek()
        {
            List<YazarlikBasvurusu> istek = ctx.YazarlikBasvurusus.ToList();
            return View(istek);
        }
        [HttpPost]
        public bool istekReddet(int id)
        {
            var al = ctx.YazarlikBasvurusus.FirstOrDefault(x => x.BasvuruID == id);
            ctx.YazarlikBasvurusus.Remove(al);
            int sonuc=ctx.SaveChanges();
            if (sonuc == 1)
                return true;
            else
                return false;

        }
    [HttpPost]
        public ActionResult YazarOnay(Kullanici k)
        {
            var kulAdi = ctx.YazarlikBasvurusus.FirstOrDefault(x => x.Nick == k.Nick);
            k.Adi = kulAdi.Ad;
            k.Soyadi = kulAdi.Soyad;
            k.Mail = kulAdi.mail;
            ctx.SaveChanges();
            Roles.AddUserToRole(k.Nick, "Yazar");
            ctx.YazarlikBasvurusus.Remove(kulAdi);
            ctx.SaveChanges();
            return RedirectToAction("yazarIstek");
        

        }
	}
}