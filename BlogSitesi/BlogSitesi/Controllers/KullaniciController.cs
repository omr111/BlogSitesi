using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BlogSitesi.Controllers
{
    public class KullaniciController : Controller
    {
        BlogContext ctx = new BlogContext();
        //
        // GET: /Kullanici/
    [Authorize(Roles="Moderator")]
        public ActionResult Index()
        {
            
            List<Kullanici> kul = ctx.Kullanicis.ToList();

            return View(kul);
        }                                                               
        public ActionResult GirisYap()
        {
            return View();

        }
        [HttpPost]
        public ActionResult GirisYap(string KullaniciAdi,string parola)
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
                ViewBag.Mesaj="Kullanıcı Adı veya Parola Yanlış !";
            return RedirectToAction("GirisYap");
            }
            

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
		    MembershipUser user = Membership.CreateUser(k.Nick, parola, k.Mail);
                k.id = (Guid)user.ProviderUserKey;
                k.KayitTarihi = DateTime.Now;
                Session["Kullanici"] = k;
                k.ResimID = MakaleController.ResimKaydet(Resim, HttpContext);
                k.YazarMi = false;
                Roles.AddUserToRole(k.Nick, "Uye");
                ctx.Kullanicis.Add(k);
                ctx.SaveChanges();

                FormsAuthentication.RedirectFromLoginPage(k.Adi, true);
                Session["Kullanici"] = k;
            return RedirectToAction("Index","Home");
	    }
	    catch (Exception)
	    {
		ViewBag.nickVar = "Kullanıcı İsmi Zaten Kullanılmaktadır.";
	    }
           return View();   
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
	}
}