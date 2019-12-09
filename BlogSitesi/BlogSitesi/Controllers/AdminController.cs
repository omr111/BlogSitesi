using BlogSitesi.App_Classes;
using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BlogSitesi.Controllers
{
    
    public class AdminController : Controller
    {
     
        BlogContext ctx = new BlogContext();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult YazarListesi()
        {
            List<aspnet_Users> users = ctx.aspnet_Users.Where(x => x.aspnet_Roles.Any(y => y.RoleName!="Uye")).ToList();
           
            
            //List<Kullanici> yazarlar = ctx.Kullanicis.Where(x => x.YazarMi == true).ToList();

            return View(users);
        }
        public ActionResult TumMakale()
        {
            List<Makale> makaleler = ctx.Makales.ToList();
            return View(makaleler);
        }
        public ActionResult yazarIstek()
        {
            List<YazarlikBasvurusu> istek = ctx.YazarlikBasvurusus.ToList();
            return View(istek);
        }
        [HttpPost]
        public bool istekReddet(int id)
        {
            var al = ctx.YazarlikBasvurusus.FirstOrDefault(x => x.BasvuruID == id);
            ctx.YazarlikBasvurusus.Remove(al);
            int sonuc=ctx.SaveChanges();
            if (sonuc == 1)
                return true;
            else
                return false;

        }
    [HttpPost]
        public ActionResult YazarOnay(Kullanici k)
        {
            var kulAdi = ctx.YazarlikBasvurusus.FirstOrDefault(x => x.Nick == k.Nick);
            k.Adi = kulAdi.Ad;
            k.Soyadi = kulAdi.Soyad;
            k.Mail = kulAdi.mail;
            ctx.SaveChanges();
            Roles.AddUserToRole(k.Nick, "Yazar");
            ctx.YazarlikBasvurusus.Remove(kulAdi);
            ctx.SaveChanges();
            return RedirectToAction("yazarIstek");
        }
        public ActionResult kullaniciEngelle(Guid id)
        {
            Kullanici kullanici = ctx.Kullanicis.FirstOrDefault(x => x.id == id);
            if (kullanici.Aktif)
            {
                kullanici.Aktif = false;

            }
            else
            {
                kullanici.Aktif = true;
            }
            ctx.Entry(kullanici).State = EntityState.Modified;
            int result = ctx.SaveChanges();
            if (result > 0)
            {

                return RedirectToAction("Index", "Kullanici");
            }
            else
            {
                return View();
            }
        }
        public ActionResult yazarEngelle(Guid id)
        {
            Kullanici yazar = ctx.Kullanicis.FirstOrDefault(x => x.id == id);
            if (yazar.Aktif)
            {
                yazar.Aktif = false;
            }
            else
            {
                yazar.Aktif = true;
            }

            ctx.Entry(yazar).State = EntityState.Modified;
            ctx.SaveChanges();
            return RedirectToAction("YazarListesi", "Admin");
        }

        public PartialViewResult Adds()
        {
            
            return PartialView(ctx.Reklamlars.ToList());
        }

        public ActionResult adminPanelReklamlar()
        {
            return View(ctx.Reklamlars.ToList());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult reklamEkle(Reklamlar reklam, HttpPostedFileBase file)
        {
            try
            {
               
                if (ModelState.IsValid && file != null)
                {
                    int picWidth = Setttings.ReklamSize.Width;
                    int pichHeight = Setttings.ReklamSize.Height;
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
                    Bitmap pictureDraw = new Bitmap(orjResim, picWidth, pichHeight);
                    if (Directory.Exists(Server.MapPath("/Content/addPictures/")))
                    {
                        pictureDraw.Save(Server.MapPath("/Content/addPictures/" + newName));
                    }

                    
                    
                    reklam.reklamPath = "/Content/addPictures/" + newName;
                    reklam.reklamText = newName;
                   
                    ctx.Reklamlars.Add(reklam);
                    int result = ctx.SaveChanges();
                    Session["bannerEklenemedi"] = "";
                    if (result > 0)
                    {
                        Session["bannerEklenemedi"] = "Reklam Başarıyla Eklendi";
                        return RedirectToAction("adminPanelReklamlar", "Admin");
                    }
                    else
                    {
                        Session["bannerEklenemedi"] = "Reklam Kaydı Sırasında Bir Hata Oluştu!";

                        return RedirectToAction("adminPanelReklamlar", "Admin");
                    }
                }

                else
                {
                    Session["bannerEklenemedi"] = "Lütfen İlgili Alanları Doldurunuz.";
                    return RedirectToAction("adminPanelReklamlar", "Admin");
                }
            }
            catch (Exception e)
            {
                Session["bannerEklenemedi"] = e.Message;
                return RedirectToAction("adminPanelReklamlar", "Admin");
            }
        }
        [HttpPost]
        public ActionResult reklamDelete(int id)
        {
            try
            {
                
                Reklamlar bnr = ctx.Reklamlars.FirstOrDefault(x => x.id == id);
                if (bnr != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(bnr.reklamPath)))
                    {
                        System.IO.File.Delete(Server.MapPath(bnr.reklamPath));

                    }

                    ctx.Reklamlars.Remove(bnr);
                    int resultDeleteBanner = ctx.SaveChanges();

                    if (resultDeleteBanner > 0)
                    {
                        return Json(new { id = 1, message = "Reklam Başarıyla Silindi." });
                    }
                    else
                    {
                        return Json(new { id = 0, message = "Bu Reklam Silinemedi, Başka Bir Yerde Kullanılıyor Olabilir !" });
                    }

                }
                else
                {
                    return Json(new { id = 0, message = "Silmek İstediğiniz Reklam Bulunamadı!" });
                }


            }
            catch (Exception e)
            {
                return Json(new { id = 0, message = e.Message });
            }
        }

        public PartialViewResult sponsorWidget()
        {
            return PartialView(ctx.sponsorlars.ToList());
        }

        public ActionResult sponsorAdminPanel()
        {
            return View(ctx.sponsorlars.ToList());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult sponsorEkle(sponsorlar sponsor,HttpPostedFileBase file)
        {
            try
            {

                if (ModelState.IsValid && file != null)
                {
                    int picWidth = Setttings.SponsorSize.Width;
                    int pichHeight = Setttings.SponsorSize.Height;
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
                    Bitmap pictureDraw = new Bitmap(orjResim, picWidth, pichHeight);
                    if (Directory.Exists(Server.MapPath("/Content/sponsorResim/")))
                    {
                        pictureDraw.Save(Server.MapPath("/Content/sponsorResim/" + newName));
                    }



                    sponsor.sponsorPath = "/Content/sponsorResim/" + newName;


                    ctx.sponsorlars.Add(sponsor);
                    int result = ctx.SaveChanges();
                    Session["bannerEklenemedi"] = "";
                    if (result > 0)
                    {
                        Session["bannerEklenemedi"] = "Sponsor Başarıyla Eklendi";
                        return RedirectToAction("sponsorAdminPanel", "Admin");
                    }
                    else
                    {
                        Session["bannerEklenemedi"] = "Sponsor Kaydı Sırasında Bir Hata Oluştu!";

                        return RedirectToAction("sponsorAdminPanel", "Admin");
                    }
                }

                else
                {
                    Session["bannerEklenemedi"] = "Lütfen İlgili Alanları Doldurunuz.";
                    return RedirectToAction("sponsorAdminPanel", "Admin");
                }
            }
            catch (Exception e)
            {
                Session["bannerEklenemedi"] = e.Message;
                return RedirectToAction("sponsorAdminPanel", "Admin");
            }
        }
        [HttpPost]
        public  ActionResult sponsorSil(int id)
        {
            try
            {

                sponsorlar bnr = ctx.sponsorlars.FirstOrDefault(x => x.id == id);
                if (bnr != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(bnr.sponsorPath)))
                    {
                        System.IO.File.Delete(Server.MapPath(bnr.sponsorPath));

                    }

                    ctx.sponsorlars.Remove(bnr);
                    int resultDeleteBanner = ctx.SaveChanges();

                    if (resultDeleteBanner > 0)
                    {
                        return Json(new { id = 1, message = "Sponsor Başarıyla Silindi." });
                    }
                    else
                    {
                        return Json(new { id = 0, message = "Bu Sponsor Silinemedi, Başka Bir Yerde Kullanılıyor Olabilir !" });
                    }

                }
                else
                {
                    return Json(new { id = 0, message = "Silmek İstediğiniz Sponsor Bulunamadı!" });
                }


            }
            catch (Exception e)
            {
                return Json(new { id = 0, message = e.Message });
            }
        }
	}
}