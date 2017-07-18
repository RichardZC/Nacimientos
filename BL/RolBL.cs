using BE;
using BL.modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class RolBL : Repositorio<rol>
    {
        public static List<Roles> ListarRoles(int pUsuarioId)
        {
            using (var bd = new nacEntities())
            {
                var roles = bd.rol.Select(x => new Roles()
                {
                    RolId = x.RolId,
                    Denominacion = x.Denominacion,
                    Estado = false
                }).ToList();
                var asignados = bd.rol.Where(x => x.usuario.FirstOrDefault().UsuarioId == pUsuarioId);

                foreach (var a in asignados)
                {
                    foreach (var o in roles)
                    {
                        if (o.RolId == a.RolId)
                        {
                            o.Estado = true;
                            break;
                        }
                    }
                }
                return roles;
            }
        }

        public static List<Roles> ListarRoles()
        {
            using (var bd = new nacEntities())
            {
                var roles = bd.rol.Select(x => new Roles()
                {
                    RolId = x.RolId,
                    Denominacion = x.Denominacion,
                    Estado = false
                }).ToList();
                
                return roles;
            }
        }

        public static void GuardarRolMenu(rol r)
        {
            using (var bd = new nacEntities())
            {
                bd.Configuration.ProxyCreationEnabled = false; /*integridad referencial*/
                bd.Configuration.LazyLoadingEnabled = false;
                bd.Configuration.ValidateOnSaveEnabled = false;
                bd.Database.ExecuteSqlCommand("Delete from menu_rol where RolId=" + r.RolId.ToString());
                // bd.SaveChanges();

                var mnuBK = r.menu;
                r.menu = null;
                bd.Entry(r).State = EntityState.Unchanged; /*no modificar la base de datos rol */
                r.menu = mnuBK;
                foreach (var i in r.menu)
                    bd.Entry(i).State = EntityState.Unchanged; /*no modificar la base de datos menu */

                bd.SaveChanges();

                
                string qry = "insert into menu_rol (RolId,MenuId) " 
                                + "SELECT DISTINCT " + r.RolId.ToString() + ",Referencia "
                                + "from menu_rol rm "
                                + "inner join menu m on rm.MenuId = m.MenuId "
                                + "where rm.RolId=" + r.RolId.ToString() + " and IndPadre=0";
                bd.Database.ExecuteSqlCommand(qry);
            }
        }
    }
}
