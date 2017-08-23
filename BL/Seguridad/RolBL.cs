using BE;
using BL.modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
                var asignados = bd.usuario_rol.Where(x => x.UsuarioId == pUsuarioId);

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
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (var bd = new nacEntities())
                    {
                        bd.rol_menu.RemoveRange(bd.rol_menu.Where(x => x.RolId == r.RolId));
                        bd.rol_menu.AddRange(r.rol_menu);
                        bd.SaveChanges();

                        var q = (from rm in bd.rol_menu
                                 where rm.RolId == r.RolId && rm.menu.IndPadre == false
                                 select rm.menu.Referencia)
                                 .ToList().Distinct()
                                 .Select(x => new rol_menu { RolId = r.RolId, MenuId = x.Value })
                                 .ToList();

                        bd.rol_menu.AddRange(q);

                        bd.SaveChanges();
                    }
                    scope.Complete();
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
