﻿using BlogSitesi.App_Classes;
using BlogSitesi.Models;
using BlogSitesi.myModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
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


            return View(ctx.Banners.ToList());
        }
        public ActionResult MakaleListele(int? page )
        {
            int pageIndex;
            int pagingCount = 4;
            List<Makale> sendMakale = null;
            if (!page.HasValue)
            {
               sendMakale = ctx.Makales.OrderByDescending(x => x.YayinTarihi).Take(pagingCount).ToList();
              
            }
            else
            {
                pageIndex = pagingCount * page.Value;
               sendMakale = ctx.Makales.OrderByDescending(x => x.YayinTarihi).Skip(pageIndex).Take(pagingCount).ToList();
             
            }

            if (Request.IsAjaxRequest())
            {
              
                    return PartialView("_MakaleListele", sendMakale);
              
                
            }
             return View("_MakaleListele", sendMakale);
         
         
        }

        public PartialViewResult headerSection()
        {

            return PartialView(ctx.SirketBilgileris.FirstOrDefault(x=>x.id==1));
        }

        public PartialViewResult Footer()
        {
            footerModel footerModel = new footerModel();
            footerModel.SirketBilgileri = ctx.SirketBilgileris.FirstOrDefault(x => x.id == 1);
            footerModel.populerMakaleler = ctx.Makales.OrderByDescending(x => x.Goruntulenme).Take(3).ToList();
            return PartialView(footerModel);
        }
        public ActionResult sirketBilgileri()
        {
            
            return View(ctx.SirketBilgileris.FirstOrDefault(x => x.id ==1));
        }

        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult sirketBilgileri(SirketBilgileri sirket,HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file==null)
                    {
                        return Json(new {info=1, message = "Lütfen Makaleniz İçin Bir Resim Seçiniz!"});
                    }
                    else
                    {

                        if (System.IO.File.Exists(Server.MapPath( sirket.logoPath)))
                        {
                            System.IO.File.Delete(Server.MapPath(sirket.logoPath));

                        }
                     


                        int logoWidth = Setttings.LogoSize.Width;
                        int logoHeight = Setttings.LogoSize.Height;

                        string newName = "";
                        if (file.FileName.Length > 10)
                        {
                            newName = Path.GetFileNameWithoutExtension(file.FileName.Substring(0, 10)) + "-" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                        }
                        else
                        {
                            newName = Path.GetFileNameWithoutExtension(file.FileName) + "-" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                        }
                        Image orjResim = Image.FromStream(file.InputStream);
                        Bitmap logoDraw = new Bitmap(orjResim, logoWidth, logoHeight);
                        logoDraw.Save(Server.MapPath("~/Content/logo/" + newName));


                        sirket.logoPath = "/Content/logo/" + newName;
                        ctx.SirketBilgileris.AddOrUpdate(sirket);
                        ctx.SaveChanges();

                    }

                    return RedirectToAction("sirketBilgileri");
                }
                else
                {
                    return View(ctx.SirketBilgileris.FirstOrDefault(x => x.id == 1));
                }

            }
            catch (Exception e)
            {
                return Json(new {info=1, message = e.Message});
            }

        }
        
	}
}