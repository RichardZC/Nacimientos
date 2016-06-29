using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.Mvc;
using BL;
using BE;
using App.Models;
using System.Web;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var NroNac = NacimientoBL.Contar();
            var NroDef = DefuncionBL.Contar();
            var NroMat = MatrimonioBL.Contar();

            ViewBag.NroNac = NroNac;
            ViewBag.NroDef = NroDef;
            ViewBag.NroMat = NroMat;
            ViewBag.Total = NroNac + NroDef + NroMat;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Autenticar()
        {
            string login = Request.Form["username"].Trim();
            string pass = Request.Form["password"].Trim();
            bool remenber = false;
            if (Request.Form.Count>2)          
                remenber = true;

            var res = new ResponseModel() { error = string.Empty, respuesta = true };

            var aut = UsuarioBL.Obtener(x => x.Nombre == login && x.Clave == pass);
            if (aut != null)
            {
                FormsAuthentication.SetAuthCookie(login, remenber);
                res.valor = aut.Nombre;
                res.valor1 = aut.UsuarioId.ToString();
            }
            else {
                res.error = "Usuario o Password Incorrecto, Intente nuevamente!";
                res.respuesta = false;
            }
            return Json(res);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Anexo(int DefuncionId)
        {
            var rpt = new ResponseModel
            {
                respuesta = true,
                error = ""
            };

            
                rpt.respuesta = false;
                rpt.error = "Debe adjuntar un documento";
           
            return Json(rpt);
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