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
    
    public partial class Tbl_FormStep
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_FormStep()
        {
            this.Tbl_Question = new HashSet<Tbl_Question>();
        }
    
        public int FS_ID { get; set; }
        public System.Guid FS_Guid { get; set; }
        public int FS_FormID { get; set; }
        public string FS_Name { get; set; }
        public int FS_Order { get; set; }
        public string FS_Display { get; set; }
        public bool FS_IsDelete { get; set; }
    
        public virtual Tbl_Form Tbl_Form { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Question> Tbl_Question { get; set; }
    }
}
