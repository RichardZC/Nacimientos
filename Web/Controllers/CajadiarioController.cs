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
            var c = CajaBL.Listar(x => x.IndBoveda == false && x.Estado, includeProperties: "cajadiario,cajadiario.persona");
            return View(c);
        }

        public JsonResult ObtenerSaldoBoveda()
        {
            return Json(CajadiarioBL.ObtenerSaldoBoveda(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(int pCajaId, int pPersonaId, decimal SaldoInicial = 0)
        {
            var rm = new ResponseModel();
            var saldo = BL.CajadiarioBL.ObtenerSaldoBoveda();
            if (SaldoInicial > saldo)
            {
                rm.SetResponse(false, "El saldo inicial ingresado es incorrecto");
                return Json(rm);
            }

            try
            {
                int cajaDiarioId = CajadiarioBL.AsignarCajero(pCajaId, pPersonaId, SaldoInicial);
                var cd = CajadiarioBL.Obtener(x => x.CajaDiarioId == cajaDiarioId, includeProperties: "Persona");
               
                
                rm.SetResponse(true);
                rm.function = "RefreshRowOf(" + pCajaId + ",'" + cd.persona.NombreCompleto + "','" + cd.FechaInicio + "'," + cd.SaldoInicial + "," + cd.Entradas + "," + cd.Salidas + "," + cd.SaldoFinal + ");fn.notificar();";
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
        [HttpPost]
        public JsonResult GuardarCaja(int cajaId, string nombre)
        {
            nombre = nombre.ToUpper();
            var personaid = UsuarioBL.Obtener(Comun.SessionHelper.GetUser()).PersonaId.Value;
            if (CajaBL.Contar(x => x.IndBoveda && x.Estado) == 0)
            {
                var c = new caja { Denominacion = "BOVEDA", IndAbierto = true, IndBoveda = true, Estado = true };
                CajaBL.Crear(c);
                CajadiarioBL.Crear(new cajadiario { CajaId = c.CajaId, PersonaId = personaid, FechaInicio = DateTime.Now, IndAbierto = true });
            }

            if (cajaId == 0)
                CajaBL.Crear(new caja { Denominacion = nombre, IndAbierto = false, IndBoveda = false, Estado = true });
            else
                CajaBL.ActualizarParcial(new caja { CajaId = cajaId, Denominacion = nombre }, x => x.Denominacion);

            return Json(true);
        }

        public JsonResult ComboUsuariosCajaAsignar()
        {
            return Json(BL.UsuarioBL.ListarUsuariosSinCaja()
                .Select(x => new { Id = x.PersonaId, Valor = x.NombreCompleto })
                , JsonRequestBehavior.AllowGet);
        }
        public JsonResult ContarUsuariosCajaAsignar() {
            return Json(BL.UsuarioBL.ListarUsuariosSinCaja().Count, JsonRequestBehavior.AllowGet);
        }
    }
}