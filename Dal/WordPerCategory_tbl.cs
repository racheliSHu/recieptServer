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
    
    public partial class WordPerCategory_tbl
    {
        public int ID { get; set; }
        public Nullable<int> category { get; set; }
        public Nullable<int> word { get; set; }
        public Nullable<int> countWord { get; set; }
    
        public virtual Category_tbl Category_tbl { get; set; }
        public virtual WordCategory_tbl WordCategory_tbl { get; set; }
    }
}
