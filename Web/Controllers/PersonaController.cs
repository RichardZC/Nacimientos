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
            var per = new persona();
            if (id == 0)
            {
                //var max = NacimientoBL.ObtenerUltimoLibro();
                //nac.NroLibro = max[0];
                //nac.NroActa = max[1] + 1;
                //nac.Fecha = DateTime.Now;
                //nac.Sexo = "M";
            }
            else
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

            if (per.PersonaId == 0)
            {
                //nac.Url = string.Empty;
                PersonaBL.Crear(per);

            }
            else
            {

                PersonaBL.Actualizar(per);
            }

            rm.SetResponse(true);
            rm.function = "fn.notificar()";
            rm.result = per.PersonaId.ToString();
            
            return Json(rm);
        }
    }
}