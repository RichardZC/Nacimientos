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
    
    public partial class nacimiento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public nacimiento()
        {
            this.nacimiento_anexo = new HashSet<nacimiento_anexo>();
        }
    
        public int NacimientoId { get; set; }
        public string ApellidoNombre { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Sexo { get; set; }
        public int NroActa { get; set; }
        public int NroLibro { get; set; }
        public string Url { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nacimiento_anexo> nacimiento_anexo { get; set; }
    }
}
