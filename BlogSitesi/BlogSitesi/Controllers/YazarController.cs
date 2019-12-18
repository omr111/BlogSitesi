using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSitesi.Controllers
{
    [AllowAnonymous]
    public class YazarController : Controller
    {
        u9139968_blogContext ctx = new u9139968_blogContext();
        //
        // GET: /Yazar/
        public ActionResult Index(Guid id)
        {
            return View(id);
        }

        public ActionResult MakaleListele(Guid id)
        {
            var data = ctx.Makales.Where(x => x.YazarID == id);
            return View("_MakaleListele",data);

        }
        public ActionResult YazarinMakaleleri()
        {
            var aktifKullanici=User.Identity.Name;
            var kulid = ctx.Kullanicis.FirstOrDefault(x => x.Nick == aktifKullanici).id;
            List<Makale> YazarMakaleleri = ctx.Makales.Where(x => x.YazarID == kulid).ToList();
            ViewBag.kullaniciId = kulid;
            return View(YazarMakaleleri);
        }

        public ActionResult MakaleKilitle(int id)
        {
            Makale makale = ctx.Makales.FirstOrDefault(x => x.id == id);
            if (makale.Aktif)
            {
                makale.Aktif = false;

            }
            else
            {
                makale.Aktif = true;
            }
            ctx.Entry(makale).State = EntityState.Modified;
            int result = ctx.SaveChanges();
            if (result > 0)
            {

                return RedirectToAction("YazarinMakaleleri", "Yazar");
            }
            else
            {
                return View();
            }
        }
	}
}