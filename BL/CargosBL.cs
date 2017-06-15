using BE;
using BL.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CargosBL:Repositorio<cargos>
    {
        //public static List<Cargos> ListarCargo(int CargosId)
        //{
        //    using (var bd = new nacEntities())
        //    {
        //        var cargos = bd.cargos.Select(x => new Cargos()
        //        {
        //            CargosId = x.CargosId,
        //            Denominacion = x.Denominacion,
        //            Estado = false
        //        }).ToList();
        //        var asignados = bd.cargos.Where(x => x.usuario.FirstOrDefault().UsuarioId == CargosId);

        //        foreach (var a in asignados)
        //        {
        //            foreach (var o in cargos)
        //            {
        //                if (o.CargosId == a.CargosId)
        //                {
        //                    o.Estado = true;
        //                    break;
        //                }
        //            }
        //        }
        //        return cargos;
        //    }
        //}
    }
}
