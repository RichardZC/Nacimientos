using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public  class ComunBL
    {
        public static int GetPersonaIdSesion()
        {
            return UsuarioBL.Obtener(Comun.SessionHelper.GetUser()).PersonaId.Value;
        }
        public static int GetCajaDiarioId()
        {
            using (var bd = new nacEntities())
            {
                var personaid = bd.usuario.Find(Comun.SessionHelper.GetUser()).PersonaId;
                return bd.cajadiario
                    .First(x => x.PersonaId == personaid && x.IndAbierto && x.caja.IndBoveda == false && x.caja.IndAbierto)
                    .CajaDiarioId;
            }            
        }
        public static int GetBovedaCajaDiarioId()
        {
            using (var bd = new nacEntities())
            {
                return bd.cajadiario
                    .First(x => x.IndAbierto && x.caja.IndBoveda && x.caja.IndAbierto)
                    .CajaDiarioId;
            }
        }
    }
}
