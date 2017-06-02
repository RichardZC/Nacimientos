using BE;
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
            var usuario = UsuarioBL.Obtener(x => x.Nombre == username && x.Clave == password && x.Activo);
            if (usuario != null)
            {
                if (usuario.IndCambio)
                {
                    rm.SetResponse(true);
                    rm.function = "CambiarClave("+ usuario.UsuarioId + ");";
                }
                else
                {
                    SessionHelper.AddUserToSession(usuario.UsuarioId.ToString());
                    rm.SetResponse(true);
                    rm.href = Url.Action("Index", "Home");
                    rm.function = "$.ajax({url:'Login/_CargarMenu',dataType:'html',success: function(d) {localStorage.setItem('mnu', d)} });";
                }

            }
            else
            {
                rm.SetResponse(false, "Usuario o Clave Incorrecta");
            }
            return Json(rm);
        }

        public ActionResult Logout()
        {
            SessionHelper.DestroyUserSession();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult CambiarClave(int usuarioId)
        {
            ViewBag.UsuarioId = usuarioId;
            return PartialView();
        }
        [HttpPost]
        public JsonResult ActualizarClave(int usuarioId, string clave, string clave1) {


            var rm = new ResponseModel();
            if (clave!= clave1)
            {
                rm.SetResponse(false, "Las Claves Son Diferentes");
                return Json(rm);
            }

            try
            {
                var enc = Comun.HashHelper.MD5(clave);
                UsuarioBL.ActualizarParcial(new usuario { UsuarioId = usuarioId, Clave = enc, IndCambio = false }, x => x.Clave, x => x.IndCambio);

                SessionHelper.AddUserToSession(usuarioId.ToString());
                rm.SetResponse(true);
                rm.href = Url.Action("Index", "Home");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, ex.Message);
            }

            return Json(rm);
        }

        public ActionResult _CargarMenu()
        {
            return PartialView();
        }

    }
}