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
    public class CajadiarioController : Controller
    {
        // GET: Cajadiario
        public ActionResult Index()
        {
            return View(CajaBL.Listar(x => x.IndBoveda == false && x.Estado, includeProperties: "persona"));
        }
      
        public JsonResult ObtenerSaldoBoveda()
        {
            return Json(CajadiarioBL.ObtenerSaldoBoveda(),JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(int pCajaId, int pPersonaId,decimal SaldoInicial=0)
        {
            var rm = new ResponseModel();


            var saldo = BL.CajadiarioBL.ObtenerSaldoBoveda();
            if (SaldoInicial > saldo)
            {
                rm.SetResponse(false, "El saldo inicial ingresado es incorrecto");
                return Json(rm);
            }
            


            cajadiario cd = new cajadiario();
            cd.CajaId = pCajaId;
            cd.PersonaId = pPersonaId;
            cd.SaldoInicial = SaldoInicial;
            cd.FechaInicio = DateTime.Now;
            cd.IndAbierto = true;
            cd.Entradas = 0;
            cd.Salidas = 0;
            cd.SaldoFinal = 0;

           

            try
            {
                CajadiarioBL.Guardar(cd);

                //actualizando la caja
                CajaBL.ActualizarParcial(new caja { CajaId = pCajaId, IndAbierto = true, PersonaId = pPersonaId,FechaInicioOperacion=cd.FechaInicio }, x => x.IndAbierto, x => x.PersonaId,x => x.FechaInicioOperacion);

                //actualizando la boveda
                //CajadiarioBL.ActualizarParcial(new cajadiario { CajaDiarioId = boveda.CajaDiarioId,  SaldoFinal = (boveda.SaldoFinal-SaldoInicial)},  x => x.SaldoFinal);

                persona p = PersonaBL.Obtener(x => x.PersonaId== pPersonaId);
                rm.SetResponse(true);

                rm.function = "RefreshRowOf("+ pCajaId + ",'" + p.NombreCompleto + "','"+cd.FechaInicio+"');fn.notificar();";
            }
            catch (Exception ex)
            {
                rm.SetResponse(false);
                rm.function = "fn.mensaje('" + ex.Message + "')";
            }

            return Json(rm);
        }

        public static decimal? MontoFinalBoveda()
        {
            var cajasDiarios = CajadiarioBL.Listar(x => x.IndAbierto == true, includeProperties: "Caja");
            foreach (var c in cajasDiarios)
            {
                if (c.caja.IndBoveda)
                {
                    return c.SaldoFinal;
                }
            }
            return -1;
        }
    }
}