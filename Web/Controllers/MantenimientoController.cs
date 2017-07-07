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
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult Index()
        {
            ViewBag.lstOficina = OficinaBL.Listar().Select(x => new BL.Modelo.Tabla { id = x.OficinaId, Denominacion = x.Denominacion }).ToList();
            ViewBag.lstCargo = CargoBL.Listar().Select(x => new BL.Modelo.Tabla { id = x.CargoId, Denominacion = x.Denominacion }).ToList();
            return View();
        }
        [HttpPost]
        public JsonResult Guardar(string tabla,BL.Modelo.Tabla data)
        {
            data.Denominacion = data.Denominacion.ToUpper();
            switch (tabla)
            {
                case "OFICINA":
                    var o = new BE.oficina { OficinaId = data.id, Denominacion = data.Denominacion };
                    OficinaBL.Guardar(o);
                    return Json(o.OficinaId);
                case "CARGO":
                    var c = new BE.cargo { CargoId = data.id, Denominacion = data.Denominacion };
                    CargoBL.Guardar(c);
                    return Json(c.CargoId);
            }
            return Json(0);
        }
       

    }
}