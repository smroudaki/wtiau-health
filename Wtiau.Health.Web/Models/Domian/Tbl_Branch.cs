//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wtiau.Health.Web.Models.Domian
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Branch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Branch()
        {
            this.Tbl_StudentInfo = new HashSet<Tbl_StudentInfo>();
        }
    
        public int Branch_ID { get; set; }
        public System.Guid Branch_Guid { get; set; }
        public Nullable<int> Branch_GroupID { get; set; }
        public string Branch_Display { get; set; }
        public bool Branch_IsDelete { get; set; }
        public int Branch_GradeID { get; set; }
    
        public virtual Tbl_Grad Tbl_Grad { get; set; }
        public virtual Tbl_Group Tbl_Group { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_StudentInfo> Tbl_StudentInfo { get; set; }
    }
}