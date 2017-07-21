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
    }
}
