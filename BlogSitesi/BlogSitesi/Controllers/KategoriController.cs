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
        [HttpPost]
        public ActionResult kategoriEkle(string kat)
        {
            if (ctx.Kategoris.Any(x => x.Adi == kat.ToLower()))
            {
                return Json(new {id = 0, message="Böyle bir kategori zaten mevcut"});
            }
            else if (kat.Length > 50 || string.IsNullOrEmpty(kat.Trim()))
            {
                return Json(new { id = 0, message = "Boş geçilemez ve 50 karakterden fazla giremezsiniz!" });
            }
            else
            {
                Kategori kategori=new Kategori();
                kategori.Adi = kat.ToLower();
                ctx.Kategoris.Add(kategori);
                ctx.SaveChanges();
                return Json(new { id = 1,katId=kategori.id,katAd=kategori.Adi, message = "Kategori Başarıyla Kayıt Edildi" });

            }
            
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