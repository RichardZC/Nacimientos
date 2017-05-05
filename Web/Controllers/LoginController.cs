using BL;
using Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
   
    public class LoginController : Controller
    {
        [NoLogin]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Autenticar(string username, string password)
        {
            var rm = new ResponseModel();
            password = Comun.HashHelper.MD5(password);
            var usuario = UsuarioBL.Obtener(x => x.Nombre == username && x.Clave == password);
            if (usuario != null)
            {
                SessionHelper.AddUserToSession(usuario.UsuarioId.ToString());
                rm.SetResponse(true);
                rm.href = Url.Action("Index","Home");
            }
            else {
                rm.SetResponse(false, "Usuario o Clave Incorrecta");
            }
            return Json(rm);
        }

        public ActionResult Logout()
        {
            SessionHelper.DestroyUserSession();
            return RedirectToAction("Index","Login");
        }

    }
}