using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class CajadiarioController : Controller
    {
        // GET: Cajadiario
        public ActionResult Index()
        {
            return View(CajadiarioBL.Listar(includeProperties: "caja"));
        }
    }
}