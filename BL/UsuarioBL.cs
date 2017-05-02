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


    }
}
