using BE;
using BL.modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class OficinaBL : Repositorio<oficina>
    {
        //public static List<Oficinas> ListarOficinas(int pUsuarioId)
        //{
        //    using (var bd = new nacEntities())
        //    {
        //        var oficinas = bd.oficina.Select(x => new Oficinas()
        //        {
        //            OficinaId = x.OficinaId,
        //            Denominacion = x.Denominacion,
        //            Estado = false
        //        }).ToList();
        //        var asignados = bd.oficina.Where(x => x.usuario.FirstOrDefault().UsuarioId == pUsuarioId);

        //        foreach (var a in asignados)
        //        {
        //            foreach (var o in oficinas)
        //            {
        //                if (o.OficinaId == a.OficinaId)
        //                {
        //                    o.Estado = true;
        //                    break;
        //                }
        //            }
        //        }
        //        return oficinas;
        //    }
        //}
        public static List<Oficinas> ListarOficinas()
        {
            using (var bd = new nacEntities())
            {
                var oficinas = bd.oficina.Select(x => new Oficinas()
                {
                    OficinaId = x.OficinaId,
                    Denominacion = x.Denominacion,
                    Estado = false
                }).ToList();
                
                return oficinas;
            }
        }

    }
}
