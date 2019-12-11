using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using BlogSitesi.App_Classes;
using Image = System.Drawing.Image;
using System.IO;

namespace BlogSitesi.Controllers
{
    public class KullaniciController : Controller
    {
        BlogContext ctx = new BlogContext();
        //
        // GET: /Kullanici/
        [AllowAnonymous]
        public ActionResult Index()
        {
            
            List<Kullanici> kul = ctx.Kullanicis.ToList();

            return View(kul);
        }                                                               
        [AllowAnonymous]
        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            Session["Kullanici"] = null;
            return RedirectToAction("Index","Home");
        }
        [AllowAnonymous]
        public ActionResult KayitOl()
        {
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult KayitOl(Kullanici k,HttpPostedFileBase Resim)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ctx.Kullanicis.Any(x => x.Nick == k.Nick) == false)
                    {

                        MembershipUser user = Membership.CreateUser(k.Nick, k.parola, k.Mail);

                        k.id = (Guid)user.ProviderUserKey;
                        k.KayitTarihi = DateTime.Now;
                        Session["Kullanici"] = k;
                        k.YazarMi = false;
                        ctx.Kullanicis.Add(k);
                        ctx.SaveChanges();
                        Roles.AddUserToRole(k.Nick, "Uye");

                        //resim kaydet
                        string extention = Resim.ContentType.Split('/')[1];
                        string fileName = "f_" + Guid.NewGuid() + "." + extention;
                        Image orjImage = Image.FromStream(Resim.InputStream);
                        Bitmap paintBitmap = new Bitmap(orjImage, Setttings.KullaniciResim.Width, Setttings.KullaniciResim.Height);
                        paintBitmap.Save(Server.MapPath("~/Content/kullaniciResim/" + fileName));
                        Kullanici kullanici = ctx.Kullanicis.FirstOrDefault(x => x.id == k.id);
                        kullanici.kullaniciResimPath = "/Content/kullaniciResim/" + fileName;
                        ctx.Entry(kullanici).State = EntityState.Modified;
                        ctx.SaveChanges();
                        //resim kaydet bitti.
                        FormsAuthentication.RedirectFromLoginPage(k.Adi, true);
                        Session["Kullanici"] = k;
                        return RedirectToAction("login", "Kullanici");
                    }
                    else
                    {

                        ViewData["userError"] = "Kayıt Etmeye Çalıştığınız Kullanıcı Zaten Kayıtlıdır.";

                        return View();
                    }

                }

                catch (Exception ex)
                {

                    ViewData["userError"] = "Kayıt Sırasında Bir Hata Meydana Geldi!";
                    return View();
                }
            }
            else
            {
                return View();
            }
                
      }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Moderator")]
        [HttpPost]
      public string uyeRolleri(string Nick)
        {
            List<string> roller = Roles.GetRolesForUser(Nick).ToList();
            
            string rol = "";
            foreach (string r in roller)
            {
                rol += r + "-";

            }
            rol = rol.Remove(rol.Length - 1, 1);
            return rol;
        }
      [AllowAnonymous]
        public ActionResult YazarOl()
        {
          
          return View();

        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult YazarOl(YazarlikBasvurusu k)
        {
            if (ModelState.IsValid)
            {
                var kulname = User.Identity.Name;
                var kulID = ctx.Kullanicis.FirstOrDefault(x => x.Nick == kulname);
                if (kulID != null)
                {

                    k.kID = kulID.id;
                    k.Nick = kulname;

                    ctx.YazarlikBasvurusus.Add(k);
                    ctx.SaveChanges();
                }
                else
                {
                    ViewBag.kayitYok = "Yazarlık Başvurusundan Önce Lütfen Sitemize Kayıt Olun ve Siteye Giriş Yapın!";

                }
                return View();
            }
            else
            {
                return View("YazarOl");
            }

        }
        [AllowAnonymous]
        public ActionResult login()
        {
            return View();
        }
        [AllowAnonymous]
       [HttpPost]
        public ActionResult login(string KullaniciAdi, string parola)
        {
            if (Membership.ValidateUser(KullaniciAdi,parola))
            {
             
                FormsAuthentication.RedirectFromLoginPage(KullaniciAdi,true);
                Guid GuidId = (Guid)Membership.GetUser(KullaniciAdi).ProviderUserKey;
                Session["Kullanici"] = ctx.Kullanicis.FirstOrDefault(x => x.id == GuidId);
                Session.Timeout =Session.Timeout+ 100;
                return RedirectToAction("Index", "Home");
          
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı Adı veya Parola Yanlış !";
                return RedirectToAction("login");
            }
        }

        [Authorize]
       public ActionResult userProfile()
       {
           if (User.Identity.IsAuthenticated)
           {
               var name = User.Identity.Name;
               Kullanici user = ctx.Kullanicis.FirstOrDefault(x=>x.Nick==name);
               if (user != null)
               {
                   return View(user);
               }
               else
               {
                   return RedirectToAction("Index", "Home");
               }
           }
           else
           {
               return RedirectToAction("Index", "Home");
           }


       }
       [Authorize]
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult userProfileUpdate(string newPass, Kullanici user)
       {
           if (ModelState.IsValid)
           {
               Kullanici userName = ctx.Kullanicis.FirstOrDefault(x => x.id == user.id);
               try
               {
                   if (ModelState.IsValid)
                   {

                       if (userName != null)
                       {

                           userName.Mail = user.Mail;
                           userName.Adi = user.Adi;
                           userName.Soyadi = user.Soyadi;

                           MembershipUser changeuser = Membership.GetUser(userName.Nick);
                           changeuser.Email = userName.Mail;

                           if (!string.IsNullOrEmpty(newPass))
                           {
                               changeuser.ChangePassword(userName.parola, newPass);
                               userName.parola = newPass;
                           }
                           Membership.UpdateUser(changeuser);
                           ctx.Kullanicis.AddOrUpdate(userName);
                           ctx.SaveChanges();
                           Session["Kullanici"] = user;
                           FormsAuthentication.RedirectFromLoginPage(user.Nick, true);



                           return RedirectToAction("userProfile");
                       }
                       else
                       {
                           return RedirectToAction("login", "Kullanici");
                       }

                   }
                   return View("userProfile", userName);

               }
               catch (Exception e)
               {
                   ViewData["userProfileError"] = "Güncelleme Sırasında Bir Hata Meydana Geldi.";
                   return View("userProfile", userName);
               }

           }
           else
           {
               return View("userProfile");
           }
       }
       [Authorize]
       [HttpPost]
       [ValidateAntiForgeryToken]
       public ActionResult pictureUpdate(Guid id, HttpPostedFileBase Resim)
       {
           Kullanici user = ctx.Kullanicis.FirstOrDefault(x=>x.id==id);
           if (user != null)
           {
               if (Resim != null)
               {

                   if (System.IO.File.Exists(Server.MapPath(user.kullaniciResimPath)))
                   {
                       System.IO.File.Delete(Server.MapPath(user.kullaniciResimPath));

                   }
                   int iconWidth = Setttings.KullaniciResim.Width;
                   int iconHeight = Setttings.KullaniciResim.Height;

                   string newName = "";
                   if (Resim.FileName.Length > 10)
                   {
                       newName = Path.GetFileNameWithoutExtension(Resim.FileName.Substring(0, 10)) + "-" + Guid.NewGuid() + Path.GetExtension(Resim.FileName);
                   }
                   else
                   {
                       newName = Path.GetFileNameWithoutExtension(Resim.FileName) + "-" + Guid.NewGuid() + Path.GetExtension(Resim.FileName);
                   }
                   Image orjResim = Image.FromStream(Resim.InputStream);
                   Bitmap iconDraw = new Bitmap(orjResim, iconWidth, iconHeight);

                   iconDraw.Save(Server.MapPath("~/Content/kullaniciResim/" + newName));

                   string saveDBPath = "/Content/kullaniciResim/" + newName;

                   user.kullaniciResimPath = saveDBPath;
                  ctx.Kullanicis.AddOrUpdate(user);
                 int resultUpdate = ctx.SaveChanges();
                   if (resultUpdate>0)
                   {
                       return RedirectToAction("userProfile");
                   }
                   else
                   {
                       return RedirectToAction("userProfile");
                   }
               }
               else
               {
                   return RedirectToAction("userProfile");
               }

           }
           else
           {
               return RedirectToAction("login", "Kullanici");
           }

       }

        //[HttpPost]
        //public ActionResult resetPassword(string changeName)
        //{
        //    try
        //    {
        //        ICompanyInformationBll _companyInformation = new CompanyInformationBll(new CompanyInformationDal());
        //        CompanyInformation company = _companyInformation.GetOneWitId(2);
        //        user userPass = _userBll.GetOneWithMail(changeName);

        //        if (userPass != null)
        //        {
        //            Random random = new Random();
        //            int sifreUret = random.Next(15689, 99586);

        //            userPass.password = sifreUret.ToString();
        //            bool resultUpdate = _userBll.Update(userPass);

        //            if (resultUpdate)
        //            {

        //                user reUser = _userBll.GetOneWithMail(changeName);
        //                // mail adresi ve şifresi ne ise adminpanelden company information'dan mail ve şifreyi de aynısını yapmalı!
        //                var senderEmail = new MailAddress(company.email.Trim(), "");
        //                var receiverEmail = new MailAddress(userPass.email.Trim(), "Receiver");

        //                var password = company.emailPassword.Trim();
        //                var sub = "Ayha.Net Şifre Reset";
        //                var body = string.Format("Yeni Şifreniz {0}", reUser.password);


        //                var smtp = new SmtpClient
        //                {
        //                    Timeout = 10000,
        //                    Host = "mail.ayha.net",
        //                    Port = 587,
        //                    EnableSsl = false,
        //                    DeliveryMethod = SmtpDeliveryMethod.Network,
        //                    UseDefaultCredentials = true,
        //                    Credentials = new NetworkCredential(senderEmail.Address, password),

        //                };
        //                using (var mess = new MailMessage(senderEmail, receiverEmail)
        //                {
        //                    IsBodyHtml = true,
        //                    BodyEncoding = UTF8Encoding.UTF8,
        //                    Subject = sub,
        //                    Body = body,
        //                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,

        //                })
        //                {
        //                    smtp.Send(mess);
        //                }

        //                return Json("Yeni Şifreniz Mail Adresinize Gönderildi.");
        //            }
        //            else
        //            {

        //                return Json("Mail Gönderilemedi Lütfen Tekrar Deneyiniz.");
        //            }


        //        }
        //        else
        //        {

        //            return Json("Girilen Mail Adresi Kullanılmıyor !");
        //        }

        //    }



        //    catch (Exception EX_NAME)
        //    {
        //        ViewData["resetInfo"] = "Girilen Mail Adresi Kullanılmıyor !";
        //        return View("Index");
        //    }


        //}

    }
}