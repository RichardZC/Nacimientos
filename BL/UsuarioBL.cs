using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using MySql.Data.MySqlClient;
using System.Data.Entity;

namespace BL
{
   public class UsuarioBL:Repositorio<usuario>
    {

        public static void GuardarUsuarioOficinas(usuario u)
        {
            using (var bd = new nacEntities())
            {
                bd.Configuration.ProxyCreationEnabled = false;
                bd.Configuration.LazyLoadingEnabled = false;
                bd.Configuration.ValidateOnSaveEnabled = false;

                bd.Database.ExecuteSqlCommand("Delete from usuario_oficina where UsuarioId=" + u.UsuarioId.ToString());
                
                var oficinaBK = u.oficina;
               
                u.oficina = null;
                bd.Entry(u).State = EntityState.Unchanged;
                u.oficina = oficinaBK;
                foreach (var i in u.oficina)
                    bd.Entry(i).State = EntityState.Unchanged;
                
                bd.SaveChanges();
            }
        }

        public static void GuardarUsuarioRoles(usuario u)
        {
            using (var bd = new nacEntities())
            {
                bd.Configuration.ProxyCreationEnabled = false;
                bd.Configuration.LazyLoadingEnabled = false;
                bd.Configuration.ValidateOnSaveEnabled = false;

                bd.Database.ExecuteSqlCommand("Delete from usuario_rol where UsuarioId=" + u.UsuarioId.ToString());

                var rolBK = u.rol;

                u.rol = null;
                bd.Entry(u).State = EntityState.Unchanged;
                u.rol = rolBK;
                foreach (var i in u.rol)
                    bd.Entry(i).State = EntityState.Unchanged;

                bd.SaveChanges();
            }
        }

        public static List<usuario> ListarUsuariosSinCaja()
        {

            using (var bd = new nacEntities())
            {
                var usuarios = UsuarioBL.Listar(x=>x.Activo==true, includeProperties: "persona").OrderBy(x=>x.persona.NombreCompleto);
                var asignados = bd.cajadiario.Where(x => x.IndAbierto == true);

                List<usuario> lista = new List<usuario>();
                bool contiene;
                foreach (var u in usuarios)
                {
                    contiene = false;
                    foreach (var a in asignados)
                    {
                       
                        if (u.PersonaId == a.PersonaId)
                        {
                            contiene = true;
                            break;
                        }
                    }
                    if (!contiene)
                    {
                        lista.Add(u);
                    }
                }
                return lista;
            }
        }
    }
}
