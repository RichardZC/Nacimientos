using BE;
using BL;
using BL.modelo;
using Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    [Autenticado]
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


        [HttpPost]
        public JsonResult GuardarOficinas(usuario u, int[] o = null)
        {
            var rm = new ResponseModel();
            if (o != null)
            {
                foreach (var i in o)
                    u.oficina.Add(new oficina { OficinaId = i });
            }

            try
            {
                UsuarioBL.GuardarUsuarioOficinas(u);
                rm.SetResponse(true);
                rm.function = "fn.notificar()";
            }
            catch (Exception ex)
            {
                rm.SetResponse(false);
                rm.function = "fn.mensaje('" + ex.Message + "')";
            }           

            return Json(rm);
        }

        [HttpPost]
        public JsonResult GuardarRoles(usuario u, int[] r = null)
        {
            var rm = new ResponseModel();
            if (r != null)
            {
                foreach (var i in r)
                    u.rol.Add(new rol { RolId = i });
            }

            try
            {
                UsuarioBL.GuardarUsuarioRoles(u);
                rm.SetResponse(true);
                rm.function = "fn.notificar()";
            }
            catch (Exception ex)
            {
                rm.SetResponse(false);
                rm.function = "fn.mensaje('" + ex.Message + "')";
            }

            return Json(rm);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(usuario u,string pActivo)
        {
            var rm = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(pActivo)) u.Activo = true;
                u.Nombre = u.Nombre.Trim().ToUpper();
                UsuarioBL.ActualizarParcial(u, x => x.Nombre, x => x.Activo);
                rm.SetResponse(true);
                rm.function = "fn.notificar()";
            }
            catch (Exception ex)
            {
                rm.SetResponse(false);
                rm.function = "fn.mensaje('" + ex.Message + "')";
            }

            return Json(rm);
        }

        [HttpPost]
        public JsonResult ReiniciarClave(int id)
        {
            var rm = new ResponseModel();
            try
            {
                var enc = Comun.HashHelper.MD5("123");
                UsuarioBL.ActualizarParcial(new usuario { UsuarioId = id, Clave = enc }, x => x.Clave);
                rm.SetResponse(true);                
            }
            catch (Exception ex)
            {
                rm.SetResponse(false,ex.Message);               
            }

            return Json(rm);
        }
        
    }
}