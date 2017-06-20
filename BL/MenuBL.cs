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



        //cambio
        public static List<uvw_menus> ListarMenu(int pUsuarioId)
        {
            using (var bd = new nacEntities())
            {
                //var u = UsuarioBL.Obtener(x=>x.UsuarioId == pUsuarioId,includeProperties:"rol,rol.menu");

                //var u2 = RolBL.Listar(x => x.usuario.FirstOrDefault().UsuarioId == pUsuarioId, includeProperties: "usuario,menu");


                //bd.Database.ExecuteSqlCommand("select * from uvw_rolesUsusario where UsuarioId=" + pUsuarioId);

                var lista = MenusUsuarioBL.Listar(x => x.UsuarioId == pUsuarioId, y => y.OrderBy(z => z.Orden));

                return lista; 
            }
        }

    }
}
