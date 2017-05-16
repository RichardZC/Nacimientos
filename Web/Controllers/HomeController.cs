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
        public JsonResult listapais(string query)
        {
            var lista = new List<pais>();
            lista.Add(new pais {value= "United", data= "AE" });
            lista.Add(new pais { value = "Peru", data = "PR" });
            lista.Add(new pais { value = "Lima", data = "LM" });

            return Json(new
            {
                query = "Unit",
                suggestions = lista.Where(x=>x.value.ToLower().Contains(query.ToLower())).ToList()
            }, JsonRequestBehavior.AllowGet);
            //return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public class pais {
            public string value { get; set; }
            public string data { get; set; }

        }
    }
}