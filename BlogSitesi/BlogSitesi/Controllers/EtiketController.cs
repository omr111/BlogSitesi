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
            List<Etiket> etiketler = ctx.Etikets.OrderByDescending(x => x.MakaleEtikets.Count).Take(20).ToList();
            return PartialView(etiketler);
    
        }
     
        
        public ActionResult EtiketeGoreMakaleGetir(int id ,int? page)
        {
            int pageIndex;
            int pagingCount = 4;
            List<Makale> sendMakale = null;
            if (!page.HasValue)
            {
              

                sendMakale = ctx.Makales.Where(x => x.MakaleEtikets.Any(y => y.EtiketID == id))
                    .OrderByDescending(x => x.YayinTarihi).Take(pagingCount).ToList();
            }
            else
            {
                pageIndex = pagingCount * page.Value;
                sendMakale = ctx.Makales.Where(x => x.MakaleEtikets.Any(y => y.EtiketID == id)).OrderByDescending(x => x.YayinTarihi).Skip(pageIndex).Take(pagingCount).ToList();

            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_MakaleListele", sendMakale);
            }

            return View("_MakaleListele", sendMakale);
           
        }
    }
	
}