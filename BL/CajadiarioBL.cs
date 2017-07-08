using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CajadiarioBL : Repositorio<cajadiario>
    {
        public static decimal ObtenerSaldoBoveda() {

            var cd = CajadiarioBL.Obtener(x => x.IndAbierto && x.caja.IndBoveda && x.caja.IndAbierto, includeProperties: "Caja");
            return cd.SaldoFinal;
        }
        
    }
}
