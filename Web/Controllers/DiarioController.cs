using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    [Autenticado]
    public class DiarioController : Controller
    {
        // GET: Diario
        public ActionResult Index()
        {            
            return View();
        }
    }
}