using BE;
using BL.modelo;
using System;
using System.Collections.Generic;
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
    }
}
