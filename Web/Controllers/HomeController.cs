using BL;
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
    }
}