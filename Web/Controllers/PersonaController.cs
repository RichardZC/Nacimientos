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

        public ActionResult Mantener(int id ,string href)
        {
            var per = new persona() { Sexo="M"};
            if (id > 0)
                per = PersonaBL.Obtener(id);

            if (string.IsNullOrEmpty(href)) href = string.Empty;
            ViewBag.href = href ;

            return View(per);
        }

        [HttpPost]
        public JsonResult Guardar(persona per,string href)
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
                if (string.IsNullOrEmpty(href))
                    rm.href = Url.Action("Index", "Persona");
                else
                    rm.href = href;
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