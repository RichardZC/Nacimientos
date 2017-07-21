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

            cajadiario cd = new cajadiario();
            cd.CajaId = pCajaId;
            cd.PersonaId = pPersonaId;
            cd.SaldoInicial = SaldoInicial;
            cd.FechaInicio = DateTime.Now;
            cd.IndAbierto = true;

            try
            {
                CajadiarioBL.Guardar(cd);
                CajaBL.ActualizarParcial(new caja { CajaId = pCajaId, IndAbierto = true, PersonaId = pPersonaId, FechaInicioOperacion = cd.FechaInicio }, x => x.IndAbierto, x => x.PersonaId, x => x.FechaInicioOperacion);

                if (SaldoInicial > 0)
                {
                    CajaMovBL.Crear(new cajamov
                    {
                        CajaDiarioId = cd.CajaDiarioId,
                        PersonaId = pPersonaId,
                        Operacion = "TRA",
                        Monto = SaldoInicial,
                        Glosa = "TRANS. DESDE BOVEDA",
                        IndEntrada = true,
                        Estado = "C",//concluído
                        UsuarioRegId = Comun.SessionHelper.GetUser(),
                        FechaReg = DateTime.Now
                    });
                    
                    var boveda = CajadiarioBL.Obtener(x => x.caja.IndBoveda, includeProperties: "Caja");
                    CajaMovBL.Crear(new cajamov
                    {
                        CajaDiarioId = boveda.CajaDiarioId,
                        PersonaId = ComunBL.GetPersonaIdSesion(),
                        Operacion = "TRA",
                        Monto = SaldoInicial,
                        Glosa = "TRANS. DESDE BOVEDA",
                        IndEntrada = false,
                        Estado = "C",//concluído
                        UsuarioRegId = Comun.SessionHelper.GetUser(),
                        FechaReg = DateTime.Now
                    });
                    CajadiarioBL.ActualizarParcial(new cajadiario { CajaDiarioId = boveda.CajaDiarioId, SaldoFinal = (boveda.SaldoFinal - SaldoInicial) }, x => x.SaldoFinal);
                }

                //obteniendo los datos del cajero
                persona p = PersonaBL.Obtener(x => x.PersonaId == pPersonaId);
                rm.SetResponse(true);
                rm.function = "RefreshRowOf(" + pCajaId + ",'" + p.NombreCompleto + "','" + cd.FechaInicio + "');fn.notificar();";
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
                var c = new caja { Denominacion = "BOVEDA", IndAbierto = true, IndBoveda = true, Estado = true, FechaInicioOperacion = DateTime.Now, PersonaId = personaid };
                CajaBL.Crear(c);
                CajadiarioBL.Crear(new cajadiario { CajaId = c.CajaId, PersonaId = personaid, FechaInicio = DateTime.Now, IndAbierto = true });
            }

            if (cajaId == 0)
                CajaBL.Crear(new caja { Denominacion = nombre, IndAbierto = false, IndBoveda = false, Estado = true, FechaInicioOperacion = DateTime.Now });
            else
                CajaBL.ActualizarParcial(new caja { CajaId = cajaId, Denominacion = nombre }, x => x.Denominacion);

            return Json(true);
        }
    }
}