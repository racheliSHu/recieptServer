//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class NumCompeny_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NumCompeny_tbl()
        {
            this.NumCompenyCategory_tbl = new HashSet<NumCompenyCategory_tbl>();
        }
    
        public int ID { get; set; }
        public string numCompany { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumCompenyCategory_tbl> NumCompenyCategory_tbl { get; set; }
    }
}
