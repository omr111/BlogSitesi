using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSitesi.Controllers
{
    public class KategoriController : Controller
    {
        BlogContext ctx = new BlogContext();
        //
        // GET: /Kategori/
        public ActionResult Index(int id)
        {
   
             return View(id);
        }
        public PartialViewResult KategoriWidget()
        {
            List<Kategori> k = ctx.Kategoris.ToList();
            return PartialView(k);
        }
        public ActionResult MakaleListele(int id)
        {
            var data=ctx.Makales.Where(x => x.KategoriID == id).ToList();
            return View("_MakaleListele", data);
        }
	}
}