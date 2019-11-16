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

namespace BlogSitesi.Controllers
{
    public class KullaniciController : Controller
    {
        BlogContext ctx = new BlogContext();
        //
        // GET: /Kullanici/
   
        public ActionResult Index()
        {
            
            List<Kullanici> kul = ctx.Kullanicis.ToList();

            return View(kul);
        }                                                               
       
        public ActionResult CikisYap()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
    
        public ActionResult KayitOl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KayitOl(Kullanici k,HttpPostedFileBase Resim,string parola)
        {
            try
            {
                if (ctx.Kullanicis.Any(x=>x.Nick==k.Nick)==false)
                {
                    MembershipUser user = Membership.CreateUser(k.Nick, parola, k.Mail);
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
                    Bitmap paintBitmap=new Bitmap(orjImage,Setttings.KullaniciResim.Width,Setttings.KullaniciResim.Height);
                    paintBitmap.Save(Server.MapPath("~/Content/kullaniciResim/"+fileName));
                    Kullanici kullanici = ctx.Kullanicis.FirstOrDefault(x => x.id == k.id);
                    kullanici.kullaniciResimPath = "/Content/kullaniciResim/" + fileName;
                    ctx.Entry(kullanici).State = EntityState.Modified;
                    ctx.SaveChanges();
                    //resim kaydet bitti.
                    FormsAuthentication.RedirectFromLoginPage(k.Adi, true);
                    Session["Kullanici"] = k;
                    return RedirectToAction("GirisYap","Kullanici");
                }
                else
                {
                    ViewBag.nickVar = "Kullanıcı İsmi Zaten Kullanılmaktadır.";
                    return View();
                }

            }
	
	        catch (Exception ex)
            {
                
		            ViewBag.nickVar = ex.Message;
                    return View();  
	        }
                
      }
       
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
        
        public ActionResult YazarOl()
        {
          
          return View();

        }
        [HttpPost]
        public ActionResult YazarOl(YazarlikBasvurusu k)
        {
            var kulname=User.Identity.Name;
            var kulID = ctx.Kullanicis.FirstOrDefault(x => x.Nick == kulname);
            if(kulID!=null)
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

        public ActionResult login()
        {
            return View();
        }
       [HttpPost]
        public ActionResult login(string KullaniciAdi, string parola)
        {
            if (Membership.ValidateUser(KullaniciAdi,parola))
            {
             
                FormsAuthentication.RedirectFromLoginPage(KullaniciAdi,true);
                Guid GuidId = (Guid)Membership.GetUser(KullaniciAdi).ProviderUserKey;
                Session["Kullanici"] = ctx.Kullanicis.FirstOrDefault(x => x.id == GuidId);
                return RedirectToAction("Index", "Home");
          
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı Adı veya Parola Yanlış !";
                return RedirectToAction("login");
            }
        }
    }
}