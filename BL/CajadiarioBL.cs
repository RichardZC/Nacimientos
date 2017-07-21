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

        public static bool AsignarCajero(int pCajaId, int pPersonaId, decimal SaldoInicial)
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
                        IndAbierto = true,
                        PersonaId = pPersonaId,
                        FechaInicioOperacion = cd.FechaInicio
                    }, x => x.IndAbierto, x => x.PersonaId, x => x.FechaInicioOperacion);

                    if (SaldoInicial > 0) TransferirBovedaCaja(cd.CajaDiarioId,SaldoInicial);
                    

                    scope.Complete();
                    return true;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    return false;
                }

            }
        }

        public static bool TransferirBovedaCaja(int pCajaDiarioId, decimal monto)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    CajaMovBL.Crear(new cajamov
                    {
                        CajaDiarioId = pCajaDiarioId,
                        PersonaId = 0,
                        Operacion = "TRA",
                        Monto = monto,
                        Glosa = "TRANS. DESDE BOVEDA",
                        IndEntrada = true,
                        Estado = "C",//concluído
                        UsuarioRegId = Comun.SessionHelper.GetUser(),
                        FechaReg = DateTime.Now
                    });
                    scope.Complete();
                    return true;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    return false;
                }

            }
        }
    }
}
