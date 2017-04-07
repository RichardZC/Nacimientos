using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View(UsuarioBL.Listar(includeProperties:"persona"));
        }

        public ActionResult Listar()
        {
            return Json(UsuarioBL.Listar(),JsonRequestBehavior.AllowGet);
        }

        public ActionResult parcial()
        {
            UsuarioBL.ActualizarParcial(new BE.usuario { UsuarioId = 1,Nombre="xxxx", Clave = "887788" }, x => x.Clave, x => x.Nombre);
            return Json(true, JsonRequestBehavior.AllowGet);
        }


    }
}