using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public class ConceptopagoBL
    {
        public ConceptopagoBL()
        {

        }
        public int ConceptoPagoId { get; set; }
        public string Denominacion { get; set; }
        public decimal Importe { get; set; }
        public Nullable<int> OficinaId { get; set; }
        public bool Estado { get; set; }

        public static List<conceptopago> listarConceptopago()
        {
            using (var bd = new nacEntities())
            {

                var c = bd.conceptopago.Select(x => x.Denominacion).ToList();

                return null;

                //List<usuario> lista = new List<usuario>();
                //bool contiene;
                //foreach (var u in usuarios)
                //{
                //    contiene = false;
                //    foreach (var a in asignados)
                //    {                       
                //        if (u.PersonaId == a.PersonaId)
                //        {
                //            contiene = true;
                //            break;
                //        }
                //    }
                //    if (!contiene)
                //    {
                //        lista.Add(u);
                //    }
                //}
                //return lista;

            }

        }


    }
}
