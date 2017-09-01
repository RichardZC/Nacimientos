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
    public class RolController : Controller
    {
        // GET: Oficina
        public ActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public JsonResult Guardar(oficina o) {
        //    bool Esnuevo = o.OficinaId == 0 ? true : false;
        //    o.Denominacion = o.Denominacion.ToUpper();
        //    var rm = new ResponseModel();
        //    try
        //    {
        //        OficinaBL.Guardar(o);
        //        rm.SetResponse(true);
        //        if (Esnuevo)
        //            rm.function = "AddRowOf(" + o.OficinaId + ",'" + o.Denominacion + "');fn.notificar();";
        //        else
        //            rm.function = "RefreshRowOf(" + o.OficinaId + ",'" + o.Denominacion + "');fn.notificar();";
        //    }
        //    catch (Exception ex)
        //    {
        //        rm.SetResponse(false);
        //        rm.function = "fn.mensaje('" + ex.Message + "')";
        //    }
           
        //    return Json(rm);
        //}

        public JsonResult GuardarRoles(rol r, int[] mnu)
        {
            bool Esnuevo = r.RolId == 0 ? true : false;
            r.Denominacion = r.Denominacion.ToUpper();
            var rm = new ResponseModel();

            try
            {                
                RolBL.Guardar(r);
                if (mnu != null)
                {
                    foreach (var i in mnu)
                        r.rol_menu.Add(new rol_menu { RolId = r.RolId, MenuId = i });
                }
                RolBL.GuardarRolMenu(r);

                rm.SetResponse(true);
                if (Esnuevo)
                {
                    rm.function = "AddRowOfR(" + r.RolId + ",'" + r.Denominacion + "');fn.notificar();";
                }

                else
                {
                    rm.function = "RefreshRowOfR(" + r.RolId + ",'" + r.Denominacion + "');fn.notificar();";
                }

            }
            catch (Exception ex)
            {
                rm.SetResponse(false);
                rm.function = "fn.mensaje('" + ex.Message + "')";
            }

            return Json(rm);
        }

        public JsonResult ListarMenus(int rolId)
        {
            var m = MenuBL.ListarMenu(rolId);
            return Json(m.Select(x => new { id = x.MenuId }), JsonRequestBehavior.AllowGet);
        }
    }
}