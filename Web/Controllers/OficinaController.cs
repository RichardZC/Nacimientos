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
            var rm = new ResponseModel();
            try
            {
                OficinaBL.Guardar(o);
                rm.SetResponse(true);
                if (Esnuevo)
                    rm.function = "AddRowOf(" + o.OficinaId + ",'" + o.Denominacion + "');fn.notificar();";
                else
                    rm.function = "RefreshRowOf(" + o.OficinaId + ",'" + o.Denominacion + "');fn.notificar();";
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