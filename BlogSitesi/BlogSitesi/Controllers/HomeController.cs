using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSitesi.Controllers
{
    public class HomeController : Controller
    {
        BlogContext ctx = new BlogContext();
        //
        // GET: /Home/
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult MakaleListele()
        {
            List<Makale> m = ctx.Makales.ToList();
            return View("_MakaleListele",m);
        }
        
	}
}