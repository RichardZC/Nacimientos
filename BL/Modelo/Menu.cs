using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Modelo
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Denominacion { get; set; }
        public string Modulo { get; set; }
        public string Icono { get; set; }
        public Nullable<bool> IndPadre { get; set; }
        public Nullable<int> Orden { get; set; }
        public Nullable<int> Referencia { get; set; }
        public bool Estado { get; set; }
    }
}
