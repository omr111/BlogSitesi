using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSitesi.Controllers
{
    [AllowAnonymous]
    public class YazarController : Controller
    {
        BlogContext ctx = new BlogContext();
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

       
	}
}