using BE;
using BL;
using BL.modelo;
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
            return View(UsuarioBL.Listar(includeProperties: "persona"));
        }

        public ActionResult Mantener(int id = 0)
        {
            if (id == 0)
                return View(new usuario());

            return View(new MantenerUsuario
            {
                Usuario = UsuarioBL.Obtener(x => x.UsuarioId == id, "persona"),
                Roles = RolBL.ListarRoles(id),
                Oficinas = OficinaBL.ListarOficinas(id)
            });
        }


        public class MantenerUsuario
        {
            public usuario Usuario { get; set; }
            public List<Roles> Roles { get; set; }
            public List<Oficinas> Oficinas { get; set; }
        }


        public ActionResult parcial()
        {
            UsuarioBL.ActualizarParcial(new BE.usuario { UsuarioId = 1, Nombre = "xxxx", Clave = "887788" }, x => x.Clave, x => x.Nombre);
            return Json(true, JsonRequestBehavior.AllowGet);
        }


    }
}