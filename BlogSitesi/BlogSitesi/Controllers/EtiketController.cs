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
            List<Etiket> etiketler = ctx.Etikets.Take(20).ToList();
            return PartialView(etiketler);
    
        }
        public ActionResult EtiketeGoreMakaleGetir(int id)
        {
            
            //var makale = ctx.Makales.Where(x => x.Etikets.Any(y => y.id == id));
            List<Makale> makale = ctx.Makales.Where(x => x.MakaleEtikets.Any(y => y.EtiketID == id)).ToList();
            return View("_MakaleListele", makale);
        }
	}
}