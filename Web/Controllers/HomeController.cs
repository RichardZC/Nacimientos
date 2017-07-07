using BL;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    [Autenticado]
    public class HomeController : Controller
    {
        // GET: Home
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
        public JsonResult EnvioMenus(int[] cbomenu)
        {

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult listapais(string query)
        //{
        //    return Json(new
        //    {
        //        //query = "Unit",
        //        suggestions = PersonaBL
        //                        .Listar(x => x.NombreCompleto.Contains(query))
        //                        .Select(x => new { value = x.NombreCompleto, data = x.PersonaId })
        //                        .ToList()
        //    }, JsonRequestBehavior.AllowGet);

        //}

        //public JsonResult listap()
        //{
        //    return Json(new
        //    {
        //        //query = "Unit",
        //        suggestions = PersonaBL
        //                        .Listar()
        //                        .Select(x => new { value = x.NombreCompleto, data = x.PersonaId })
        //                        .ToList()
        //    }, JsonRequestBehavior.AllowGet);

        //}

        //public JsonResult listarPersona()
        //{
        //    return Json(PersonaBL.Listar()
        //                  .Select(x => new { value = x.NombreCompleto, data = x.PersonaId })
        //                  .ToList()
        //    , JsonRequestBehavior.AllowGet);

        //}
    }
}