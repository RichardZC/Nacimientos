//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BE
{
    using System;
    using System.Collections.Generic;
    
    public partial class caja
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public caja()
        {
            this.cajadiario = new HashSet<cajadiario>();
        }
    
        public int CajaId { get; set; }
        public string Denominacion { get; set; }
        public bool IndAbierto { get; set; }
        public bool IndBoveda { get; set; }
        public bool Estado { get; set; }
        public Nullable<int> PersonaId { get; set; }
        public Nullable<System.DateTime> FechaInicioOperacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cajadiario> cajadiario { get; set; }
        public virtual persona persona { get; set; }
    }
}
