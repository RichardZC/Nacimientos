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
                        //mov origen
                        var boveda = ComunBL.GetBoveda();
                        CajaMovBL.Crear(new cajamov
                        {
                            CajaDiarioId = boveda.CajaDiarioId,
                            PersonaId = ComunBL.GetPersonaIdSesion(),
                            Monto = SaldoInicial,
                            Operacion = "TRA",
                            Glosa = "TRANS. BOVEDA A " + boveda.caja.Denominacion,
                            IndEntrada = false,
                            Estado = "T",
                            UsuarioRegId = Comun.SessionHelper.GetUser(),
                            FechaReg = DateTime.Now
                        });
                        // mov destino
                        CajaMovBL.Crear(new cajamov
                        {
                            CajaDiarioId = cd.CajaDiarioId,
                            PersonaId = cd.PersonaId.Value,
                            Monto = cd.SaldoInicial,
                            Operacion = "INI",
                            Glosa = "TRANS. SALDO INICIAL",
                            IndEntrada = true,
                            Estado = "T",
                            UsuarioRegId = Comun.SessionHelper.GetUser(),
                            FechaReg = DateTime.Now
                        });

                        ActualizarSaldoCajaDiario(boveda.CajaDiarioId);
                        ActualizarSaldoCajaDiario(cd.CajaDiarioId);
                    }

                    scope.Complete();
                    return cd.CajaDiarioId;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception(ex.Message);
                }
            }
        }




        public static bool ActualizarSaldoCajaDiario(int pCajaDiarioId)
        {
            decimal ingresoMov = 0, egresoMov = 0;

            using (var bd = new nacEntities())
            {
                ingresoMov = bd.cajamov
                    .Where(x => x.CajaDiarioId == pCajaDiarioId && x.IndEntrada && x.Estado == "T" && x.Operacion != "INI")
                    .Select(x => x.Monto).ToList().Sum();
                egresoMov = bd.cajamov
                    .Where(x => x.CajaDiarioId == pCajaDiarioId && x.IndEntrada == false && x.Estado == "T")
                    .Select(x => x.Monto).ToList().Sum();

                var cd = bd.cajadiario.Find(pCajaDiarioId);
                cd.Entradas = ingresoMov;
                cd.Salidas = egresoMov;
                cd.SaldoFinal = cd.SaldoInicial + cd.Entradas - cd.Salidas;
                bd.SaveChanges();
            }

            return true;
        }

        public static bool CrearSaldoInicial(int cajaDiarioId, decimal saldoInicial)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    CajadiarioBL.ActualizarParcial(new cajadiario { CajaDiarioId = cajaDiarioId, SaldoInicial = saldoInicial }, x => x.SaldoInicial);
                    CajaMovBL.Crear(new cajamov
                    {
                        CajaDiarioId = cajaDiarioId,
                        PersonaId = ComunBL.GetPersonaIdSesion(),
                        Monto = saldoInicial,
                        Operacion = "INI",
                        Glosa = "TRANS. SALDO INICIAL",
                        IndEntrada = true,
                        Estado = "T",
                        UsuarioRegId = Comun.SessionHelper.GetUser(),
                        FechaReg = DateTime.Now
                    });
                    ActualizarSaldoCajaDiario(cajaDiarioId);

                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception(ex.Message);
                }
            }
        }


    }
}
