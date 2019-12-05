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
        public ActionResult MakaleListele(int id,int? page)
        {
          
            int pageIndex;
            int pagingCount = 4;
            List<Makale> sendMakale = null;
            if (!page.HasValue)
            {
                sendMakale = ctx.Makales.Where(x => x.KategoriID == id).OrderByDescending(x => x.YayinTarihi).Take(pagingCount).ToList();

            }
            else
            {
                pageIndex = pagingCount * page.Value;
                sendMakale = ctx.Makales.Where(x => x.KategoriID == id).OrderByDescending(x => x.YayinTarihi).Skip(pageIndex).Take(pagingCount).ToList();

            }

            if (Request.IsAjaxRequest())
            {

                return PartialView("_MakaleListele", sendMakale);


            }
            return View("_MakaleListele", sendMakale);




          
        }
	}
}