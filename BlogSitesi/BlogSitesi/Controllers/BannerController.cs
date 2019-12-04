using BlogSitesi.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogSitesi.Models;
using System.Drawing;
using System.IO;

namespace BlogSitesi.Controllers
{
    public class BannerController : Controller
    {
        // GET: Banner
        public ActionResult Index()
        {
            BlogContext ctx=new BlogContext();
            List<Banner> banners = ctx.Banners.ToList();
            if (banners != null)
            {
                return View(banners);
            }
            return View();
        }
        [HttpPost]
        public ActionResult bannerAdd(string textArea, HttpPostedFileBase companyPicturePath)
        {

            try
            {
                BlogContext ctx=new BlogContext();
                if (ModelState.IsValid && !string.IsNullOrEmpty(textArea) && companyPicturePath != null)
                {
                    int picWidth = Setttings.BannerSize.Width;
                    int pichHeight = Setttings.BannerSize.Height;
                    string newName = "";
                    if (companyPicturePath.FileName.Length>10)
                    {
                         newName = Path.GetFileNameWithoutExtension(companyPicturePath.FileName.Substring(0,20)) + "-" + Guid.NewGuid() + Path.GetExtension(companyPicturePath.FileName);
                    }
                    else
                    {
                         newName = Path.GetFileNameWithoutExtension(companyPicturePath.FileName) + "-" + Guid.NewGuid() + Path.GetExtension(companyPicturePath.FileName);
                    }
                    Image orjResim = Image.FromStream(companyPicturePath.InputStream);
                    Bitmap pictureDraw = new Bitmap(orjResim, picWidth, pichHeight);
                    if (Directory.Exists(Server.MapPath("/Content/bannerPictures/")))
                    {
                        pictureDraw.Save(Server.MapPath("/Content/bannerPictures/" + newName));
                    }

                    Banner bnr = new Banner();
                    bnr.textArea = textArea;
                    bnr.bannerPicPath = "/Content/bannerPictures/" + newName;
                    //resmin alt'ı olarak kullanacağım filename'i
                    bnr.buttonName = companyPicturePath.FileName;
                    ctx.Banners.Add(bnr);
                    int result = ctx.SaveChanges();
                    Session["bannerEklenemedi"] = "";
                    if (result>0)
                    {
                        Session["bannerEklenemedi"] = "Banner Başarıyla Eklendi";
                        return RedirectToAction("Index", "Banner");
                    }
                    else
                    {
                        Session["bannerEklenemedi"] = "Banner Kaydı Sırasında Bir Hata Oluştu!";

                        return RedirectToAction("Index", "Banner");
                    }
                }

                else
                {
                    Session["bannerEklenemedi"] = "Lütfen İlgili Alanları Doldurunuz.";
                    return RedirectToAction("Index", "Banner");
                }
            }
            catch (Exception e)
            {
                Session["bannerEklenemedi"] = e.Message;
                return RedirectToAction("Index", "Banner");
            }




        }

        [HttpPost]
        public ActionResult bannerDelete(int id)
        {
            try
            {
                BlogContext ctx=new BlogContext();
                Banner bnr = ctx.Banners.FirstOrDefault(x=>x.id==id);
                if (bnr != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(bnr.bannerPicPath)))
                    {
                        System.IO.File.Delete(Server.MapPath(bnr.bannerPicPath));

                    }

                    ctx.Banners.Remove(bnr);
                    int resultDeleteBanner = ctx.SaveChanges();

                    if (resultDeleteBanner>0)
                    {
                        return Json(new { id = 1, message = "Banner Başarıyla Silindi." });
                    }
                    else
                    {
                        return Json(new { id = 0, message = "Bu Banner Silinemedi, Başka Bir Yerde Kullanılıyor Olabilir !" });
                    }

                }
                else
                {
                    return Json(new { id = 0, message = "Silmek İstediğiniz Banner Bulunamadı!" });
                }


            }
            catch (Exception e)
            {
                return Json(new { id = 0, message = e.Message });
            }
        }
    }
}