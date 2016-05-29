using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.Mvc;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Autenticar()
        {
            string login = Request.Form["username"].Trim();
            string pass = Request.Form["password"].Trim();
            bool remenber = false;
            if (Request.Form.Count>2)          
                remenber = true;                       
            
            if (true)
            {
                FormsAuthentication.SetAuthCookie(login, remenber);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", new { mensaje = "Usuario o Clave Incorrecto" });
        }
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Login");
        }
    }
}