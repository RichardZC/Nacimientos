using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {            
            return View(UsuarioBL.Listar(includeProperties:"persona"));
        }

        public ActionResult Mantener(int id=0)
        {
            if (id==0)            
                return View( new usuario());
            
            return View(UsuarioBL.Obtener(id));
        }
    }
}