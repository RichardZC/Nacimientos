using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    [Autenticado]
    public class OficinaController : Controller
    {
        // GET: Oficina
        public ActionResult Index()
        {
            return View(OficinaBL.Listar()  );
        }
        [HttpPost]
        public JsonResult Guardar(oficina o) {
            bool Esnuevo = o.OficinaId == 0 ? true : false;
            o.Denominacion = o.Denominacion.ToUpper();
            OficinaBL.Guardar(o);
            return Json(new { EsNuevo = Esnuevo, OficinaId = o.OficinaId });
        }
        
    }
}