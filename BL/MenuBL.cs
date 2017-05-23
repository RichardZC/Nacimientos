using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MenuBL : Repositorio<menu>
    {

        public static List<menu> ListarMenu(int pUsuarioId)
        {
            using (var bd = new nacEntities())
            {
                var usuario = UsuarioBL.Obtener(pUsuarioId);
                var rol = RolBL.Obtener();
               
                //var menus = bd.menu.Where(x => x.rol.FirstOrDefault().RolId == usuario.rol.);

                //foreach (var a in asignados)
                //{
                //    foreach (var o in roles)
                //    {
                //        if (o.RolId == a.RolId)
                //        {
                //            o.Estado = true;
                //            break;
                //        }
                //    }
                //}
                return null;
            }
        }

    }
}
