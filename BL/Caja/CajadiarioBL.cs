using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BL
{
    public class CajadiarioBL : Repositorio<cajadiario>
    {
        public static decimal ObtenerSaldoBoveda()
        {

            var cd = CajadiarioBL.Obtener(x => x.IndAbierto && x.caja.IndBoveda && x.caja.IndAbierto, includeProperties: "Caja");
            return cd == null ? 0 : cd.SaldoFinal;
        }

        public static int AsignarCajero(int pCajaId, int pPersonaId, decimal SaldoInicial)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    cajadiario cd = new cajadiario()
                    {
                        CajaId = pCajaId,
                        PersonaId = pPersonaId,
                        SaldoInicial = SaldoInicial,
                        FechaInicio = DateTime.Now,
                        IndAbierto = true
                    };
                    Guardar(cd);
                    CajaBL.ActualizarParcial(new caja
                    {
                        CajaId = pCajaId,
                        IndAbierto = true
                    }, x => x.IndAbierto);

                    if (SaldoInicial > 0)
                    {
                        CajaTransferenciaBL.Crear(new cajatransferencia
                        {
                            OrigenCajaDiarioId = ComunBL.GetBovedaCajaDiarioId(),
                            DestinoCajaDiarioId = cd.CajaDiarioId,
                            Monto = SaldoInicial,
                            Fecha = DateTime.Now,
                            Estado = "T",
                            IndSaldoInicial=true
                        });

                        ActualizarSaldoCajaDiario(ComunBL.GetBovedaCajaDiarioId());
                        ActualizarSaldoCajaDiario(cd.CajaDiarioId);
                    }

                    scope.Complete();
                    return cd.CajaDiarioId;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception( ex.Message);
                }
            }
        }




        public static bool ActualizarSaldoCajaDiario(int pCajaDiarioId)
        {
            decimal ingresoMov = 0, ingresoTra = 0, egresoMov = 0, egresoTra = 0;
           
            using (var bd = new nacEntities())
            {
                ingresoMov = bd.cajamov
                    .Where(x => x.CajaDiarioId == pCajaDiarioId && x.IndEntrada && x.Estado == "T")
                    .Select(x => x.Monto).ToList().Sum();  
                egresoMov = bd.cajamov
                    .Where(x => x.CajaDiarioId == pCajaDiarioId && x.IndEntrada == false && x.Estado == "T")
                    .Select(x => x.Monto).ToList().Sum();
                ingresoTra = bd.cajatransferencia
                    .Where(x => x.DestinoCajaDiarioId == pCajaDiarioId && x.Estado == "T" && !x.IndSaldoInicial)
                    .Select(x => x.Monto).ToList().Sum();
                egresoTra = bd.cajatransferencia
                    .Where(x => x.OrigenCajaDiarioId == pCajaDiarioId && x.Estado == "T" && x.IndSaldoInicial)
                    .Select(x => x.Monto).ToList().Sum();

                var cd = bd.cajadiario.Find(pCajaDiarioId);
                cd.Entradas = ingresoMov + ingresoTra;
                cd.Salidas = egresoMov + egresoTra;
                cd.SaldoFinal = cd.SaldoInicial + cd.Entradas - cd.Salidas;
                bd.SaveChanges();
            }
                        
            return true;
        }

    }
}
