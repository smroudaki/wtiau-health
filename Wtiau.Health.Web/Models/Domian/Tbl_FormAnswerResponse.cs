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
    
    public partial class Tbl_FormAnswerResponse
    {
        public int FAR_ID { get; set; }
        public System.Guid FAR_Guid { get; set; }
        public int FAR__FAID { get; set; }
        public int FAR_ResponseID { get; set; }
        public bool FAR_IsDelete { get; set; }
    
        public virtual Tbl_FormAnswer Tbl_FormAnswer { get; set; }
        public virtual Tbl_Response Tbl_Response { get; set; }
    }
}