//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAPI_Sistema_Cenfotec.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class seguimiento
    {
        public seguimiento()
        {
            this.bitacoras = new HashSet<bitacora>();
        }
    
        public int id_seguimiento { get; set; }
        public int id_prospecto { get; set; }
        public System.DateTime fecha { get; set; }
        public string comentario { get; set; }
        public System.DateTime fecha_creacion { get; set; }
        public System.DateTime fecha_actualizacion { get; set; }
    
        public virtual ICollection<bitacora> bitacoras { get; set; }
    }
}
