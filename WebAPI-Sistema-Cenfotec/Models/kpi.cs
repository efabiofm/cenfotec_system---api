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
    
    public partial class kpi
    {
        public int id_kpi { get; set; }
        public int id_tipo { get; set; }
        public string descripcion { get; set; }
        public string indicador { get; set; }
        public Nullable<System.DateTime> fecha_inicio { get; set; }
        public Nullable<System.DateTime> fecha_final { get; set; }
    
        public virtual tipo_kpi tipo_kpi { get; set; }
    }
}
