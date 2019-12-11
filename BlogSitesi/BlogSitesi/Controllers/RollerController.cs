using BlogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace BlogSitesi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Moderator")]
    public class RollerController : Controller
    {
        BlogContext ctx = new BlogContext();
        //
        // GET: /Roller/
        public ActionResult Index()
        {
            List<string> roller = Roles.GetAllRoles().ToList();
            ViewBag.kullanici=ctx.Kullanicis.ToList();
            return View(roller);     
        }
        [HttpPost]
        public ActionResult Index(string Nick,string RolName)
        {
            Roles.AddUserToRole(Nick,RolName);
            if (RolName!="Uye")
            {
                Kullanici kullanici = ctx.Kullanicis.FirstOrDefault(x => x.Nick == Nick);
                kullanici.YazarMi = true;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Ekle(string RolName)
        {
            if (!Roles.IsUserInRole(RolName.Trim()))
            {
                 Roles.CreateRole(RolName);

               return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

	}
}