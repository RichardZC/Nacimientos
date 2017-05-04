﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ComunController : Controller
    {
        // GET: Comun
        public ActionResult ObtenerUsuarioSesion()
        {
            return Json(Comun.SessionHelper.GetUser(),JsonRequestBehavior.AllowGet);
        }
    }
}