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
using System.Web.Services.Description;

namespace BlogSitesi.Controllers
{
    
    public class AdminController : Controller
    {

        u9139968_blogContext ctx = new u9139968_blogContext();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult YazarListesi()
        {
            List<aspnet_Users> users = ctx.aspnet_Users.Where(x => x.aspnet_Roles.Any(y => y.RoleName!="Uye")).ToList();
           
            

            return View(users);
        }
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult TumMakale()
        {
            List<Makale> makaleler = ctx.Makales.ToList();
            return View(makaleler);
        }
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult yazarIstek()
        {
            List<YazarlikBasvurusu> istek = ctx.YazarlikBasvurusus.ToList();
            return View(istek);
        }
        [Authorize(Roles = "Admin,Moderator")]
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
        [Authorize(Roles = "Admin,Moderator")]
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
        [Authorize(Roles = "Admin,Moderator")]
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
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult yorumlarKullaniciEngelle(Guid id)
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

                return RedirectToAction("tumYorumlar", "Admin");
            }
            else
            {
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
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

        [AllowAnonymous]
        public PartialViewResult Adds()
        {
            
            return PartialView(ctx.Reklamlars.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult adminPanelReklamlar()
        {
            return View(ctx.Reklamlars.ToList());
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [AllowAnonymous]
        public PartialViewResult sponsorWidget()
        {
            return PartialView(ctx.sponsorlars.ToList());
        }

        [Authorize(Roles = "Admin")]

        public ActionResult sponsorAdminPanel()
        {
            return View(ctx.sponsorlars.ToList());
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public ActionResult yorumSil(int id)
        {
            Yorum yorum = ctx.Yorums.FirstOrDefault(x => x.id == id);
            if (yorum!=null)
            {
                ctx.Yorums.Remove(yorum);
               int result= ctx.SaveChanges();
               if (result>0)
               {
                   return Json(new{id=1,message = "Yorum Başarıyla Silindi"});
               }
               else
               {
                   return Json(new {id = 0, message = "Yorum Silinemedi"});
               }
            }
            else
            {
                return Json(new {id = 1, message = "Silinmek İstenilen Yorum Bulunamadı"});

            }
        }
        
        [Authorize(Roles = "Admin")]

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
        
        [Authorize(Roles = "Admin")]

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

        [Authorize(Roles = "Admin,Moderator")]

        public ActionResult tumYorumlar()
        {
            return View(ctx.Yorums.ToList());
        }

        [Authorize(Roles = "Admin")]

        public ActionResult IstenCikart(Guid id)
        {
            try
            {
                Kullanici kullanici = ctx.Kullanicis.FirstOrDefault(x => x.id == id);
                string[] roles = Roles.GetRolesForUser(kullanici.Nick);

                Roles.RemoveUserFromRoles(kullanici.Nick.ToString(), roles);
                Roles.AddUserToRole(kullanici.Nick.ToString(),"Uye");
                return RedirectToAction("YazarListesi", "Admin");
            }
            catch (Exception e)
            {
                return RedirectToAction("YazarListesi", "Admin");
            }
          


        }

        public ActionResult MakaleKilitle(int id)
        {
            Makale makale = ctx.Makales.FirstOrDefault(x => x.id == id);
            if (makale.Aktif)
            {
                makale.Aktif = false;

            }
            else
            {
                makale.Aktif = true;
            }
            ctx.Entry(makale).State = EntityState.Modified;
            int result = ctx.SaveChanges();
            if (result > 0)
            {

                return RedirectToAction("TumMakale", "Admin");
            }
            else
            {
                return View();
            }
        }
	}
}