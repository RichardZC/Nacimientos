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
           
            return View();

        }

        public JsonResult ComboConceptopago()
        {
            return Json(BL.UsuarioBL.ListarUsuariosSinCaja()
                .Select(x => new { Id = x.PersonaId, Valor = x.NombreCompleto })
                , JsonRequestBehavior.AllowGet);
        }

    }
}