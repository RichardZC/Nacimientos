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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <param name="PersonaId">id de la persona a crear el usuario</param>
        /// <returns></returns>
        public ActionResult Mantener(int id = 0, int pPersonaId = 0)
        {

            if (id == 0)
            {

                usuario u = new usuario();
                u.Activo = true;

                persona p = PersonaBL.Obtener(pPersonaId);
                var nombreusuario = p.Nombres.Substring(0, 1) + p.Paterno;
                var cuenta = UsuarioBL.Contar(x => x.PersonaId == pPersonaId);
                if (cuenta > 0)
                    u.Nombre = nombreusuario + (cuenta + 1);
                else
                    u.Nombre = nombreusuario;

                u.PersonaId = pPersonaId;
                u.IndCambio = true;

                u.Clave = "202cb962ac59075b964b07152d234b70";//123
                UsuarioBL.Crear(u);

                id = u.UsuarioId;


            }



            return View(new MantenerUsuario
            {
                Usuario = UsuarioBL.Obtener(x => x.UsuarioId == id, "persona"),
                Roles = RolBL.ListarRoles(id),
                Oficinas = OficinaBL.ListarOficinas(id)
            });
        }

        public JsonResult listaPersonas(string query)
        {
            var lista = cargar();


            return Json(new
            {
                query = "Unit",
                suggestions = lista.Where(x => x.value.ToLower().Contains(query.ToLower())).ToList()
            }, JsonRequestBehavior.AllowGet);
            //return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public List<personita> cargar()
        {
            var lista1 = PersonaBL.Listar();
            var lista2 = new List<personita>();
            foreach (persona p in lista1)
            {
                lista2.Add(new personita { value = p.NombreCompleto, data = p.PersonaId });
            }
            return lista2;
        }

        public class personita
        {
            public string value { get; set; }
            public int data { get; set; }

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
                    u.usuario_rol.Add(new usuario_rol { UsuarioId = u.UsuarioId, RolId = i });
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
        public JsonResult GuardarUsuario(usuario u, string pActivo)
        {
            var rm = new ResponseModel();
            try
            {
                usuario u2 = UsuarioBL.Obtener(x => x.UsuarioId == u.UsuarioId);
                bool mismoUsuario = u2.Nombre.Equals(u.Nombre);
                bool yaExiste = UsuarioBL.Contar(x => x.Nombre == u.Nombre) > 0;
                if (!mismoUsuario&&yaExiste )
                {
                    rm.SetResponse(false);
                    rm.function = "fn.mensaje('El nombre de usuario ya existe.')";
                    return Json(rm); 
                }
                

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
                UsuarioBL.ActualizarParcial(new usuario { UsuarioId = id, Clave = enc, IndCambio = true }, x => x.Clave, x => x.IndCambio);
                rm.SetResponse(true);
            }
            catch (Exception ex)
            {
                rm.SetResponse(false, ex.Message);
            }

            return Json(rm);
        }

    }
}