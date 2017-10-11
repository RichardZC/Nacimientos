using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Web.Controllers
{
    public class BandejaController : Controller
    {
        // GET: Bandeja
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Mantener()
        {
            ViewBag.drpConcepto = new SelectList(BL.ConceptopagoBL.Listar(x => x.Estado), "ConceptoPagoId", "Denominacion");
            return View();

        }

       
    }
}