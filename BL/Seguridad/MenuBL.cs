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
        
        public static List<menu> ListarMenu(int rolId)
        {
            using (var bd = new nacEntities()) {
                var q = from rm in bd.rol_menu
                        where rm.RolId == rolId
                        select rm.menu;
                return q.ToList();
            }
        }

        public static List<menu> ListarMenu()
        {
            var id = Comun.SessionHelper.GetUser();
            using (var bd = new nacEntities())
            {
                var mnu = (from rm in bd.rol_menu
                           join ru in bd.usuario_rol on rm.RolId equals ru.RolId
                           where ru.UsuarioId == id
                           select rm.menu).ToList().Distinct().OrderBy(x => x.MenuId);

                return mnu.ToList();
            }
        }

        public static bool TienePermiso(string controlador)
        {
            var id = Comun.SessionHelper.GetUser();
            using (var bd = new nacEntities())
            {
                var p = (from rm in bd.rol_menu
                           join ru in bd.usuario_rol on rm.RolId equals ru.RolId
                           where ru.UsuarioId == id && rm.menu.Modulo== controlador
                           select rm.menu).Any();

                return p;
            }
        }
        public static string RolSesion()
        {
            var id = Comun.SessionHelper.GetUser();
            using (var bd = new nacEntities())
            {
                var p = (from rm in bd.rol_menu
                         join ru in bd.usuario_rol on rm.RolId equals ru.RolId
                         where ru.UsuarioId == id 
                         select ru.rol.Denominacion).FirstOrDefault();

                return p;
            }
        }
    }
}
