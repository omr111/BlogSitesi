using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
       
        public ActionResult YazarListesi()
        {
            List<aspnet_Users> users = ctx.aspnet_Users.Where(x => x.aspnet_Roles.Any(y => y.RoleName!="Uye")).ToList();
           
            
            //List<Kullanici> yazarlar = ctx.Kullanicis.Where(x => x.YazarMi == true).ToList();

            return View(users);
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
        public ActionResult kullaniciEngelle(Guid id)
        {
            Kullanici kullanici = ctx.Kullanicis.FirstOrDefault(x => x.id == id);
            if (kullanici.Aktif)
            {
                kullanici.Aktif = false;

            }
            else
            {
                kullanici.Aktif = true;
            }
            ctx.Entry(kullanici).State = EntityState.Modified;
            int result = ctx.SaveChanges();
            if (result > 0)
            {

                return RedirectToAction("Index", "Kullanici");
            }
            else
            {
                return View();
            }
        }
        public ActionResult yazarEngelle(Guid id)
        {
            Kullanici yazar = ctx.Kullanicis.FirstOrDefault(x => x.id == id);
            if (yazar.Aktif)
            {
                yazar.Aktif = false;
            }
            else
            {
                yazar.Aktif = true;
            }

            ctx.Entry(yazar).State = EntityState.Modified;
            ctx.SaveChanges();
            return RedirectToAction("YazarListesi", "Admin");
        }
	}
}