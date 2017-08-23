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
    [Autenticado]
    public class PersonaController : Controller
    {
        // GET: Persona
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Buscar(string clave = "")
        {
            return PartialView(PersonaBL.ListarPersonas(clave.Trim()));
        }

        public ActionResult Mantener(int id = 0)
        {
            var per = new persona() { Sexo="M"};
            if (id > 0)
                per = PersonaBL.Obtener(id);

            return View(per);
        }

        [HttpPost]
        public JsonResult Guardar(persona per)
        {
            var rm = new ResponseModel();
            per.Nombres = per.Nombres.ToUpper();
            per.Paterno = per.Paterno.ToUpper();
            per.Materno = per.Materno.ToUpper();
            per.NombreCompleto = per.Nombres + " " + per.Paterno + " " + per.Materno;
            try
            {
                PersonaBL.Guardar(per);                
                rm.SetResponse(true);
                rm.href = Url.Action("Index", "Persona");
            }
            catch (Exception ex)
            {
                rm.SetResponse(false);
                rm.function = "fn.mensaje('" + ex.Message + "')";
            }

            return Json(rm);
        }
    }
}