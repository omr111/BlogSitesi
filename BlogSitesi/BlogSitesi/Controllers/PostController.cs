using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSitesi.Controllers
{
    public class PostController : Controller
    {
        BlogContext ctx = new BlogContext();
        //
        // GET: /Post/
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult _PostGetir()
        {
            ViewBag.Popular = ctx.Makales.OrderByDescending(x => x.Goruntulenme).Take(5).ToList();
            ViewBag.fresh = ctx.Makales.OrderByDescending(x => x.YayinTarihi).Take(5).ToList();
            return PartialView();
        }
        public ActionResult PostMakaleGetir(int id)
        {
            Makale makale = ctx.Makales.FirstOrDefault(x => x.id == id);
            return View("Detay","Makale", makale);
        }
	}
}