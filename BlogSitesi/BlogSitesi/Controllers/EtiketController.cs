using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSitesi.Controllers
{
    public class EtiketController : Controller
    {
        BlogContext ctx = new BlogContext();
        //
        // GET: /Etiket/
        public ActionResult Index(int id)
        {
            return View(id);
        }
        public PartialViewResult Etiket()
        {
            List<Etiket> e = ctx.Etikets.ToList();
            return PartialView(e);
    
        }
        public ActionResult EtiketeGoreMakaleGetir(int id)
        {
            
            var makale = ctx.Makales.Where(x => x.Etikets.Any(y => y.id == id));
            return View("_MakaleListele", makale);
        }
	}
}