using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class OficinaController : Controller
    {
        // GET: Oficina
        public ActionResult Index()
        {
            return View(OficinaBL.Listar()  );
        }
        [HttpPost]
        public JsonResult Guardar(oficina o) {
            OficinaBL.Guardar(o);
            return Json(true);
        }
    }
}