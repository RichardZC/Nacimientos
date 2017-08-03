using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace BL
{
    public class MenuBL : Repositorio<menu>
    {

        public static List<uvw_menus> ListarMenu()
        {
            var id = Comun.SessionHelper.GetUser();
            var lista = MenusUsuarioBL.Listar(x => x.UsuarioId == id, y => y.OrderBy(z => z.MenuId));
            return lista;
        }
        public static List<menu> ListarMenu(int rolId)
        {
            using (var bd = new nacEntities()) {
                var q = from rm in bd.rol_menu
                        where rm.RolId == rolId
                        select rm.menu;
                return q.ToList();
            }
        }

        //public static List<menu> ListarMenu2()
        //{
        //    var id = Comun.SessionHelper.GetUser();
        //    using (var bd = new nacEntities())
        //    {
        //        var lista = bd.usuario
        //            .Include(x => x.rol).Include("rol.menu")
        //            .First(x => x.UsuarioId == id);
        //        var c = from u in bd.usuario
        //                select u.rol_menu.
        //        return lista;
        //    }
        //}

    }
}
