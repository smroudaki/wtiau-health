using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_PortalLogin
    {
        [Display(Name = "شماره دانشجویی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [MinLength(9,ErrorMessage ="شماره دانشجویی نامعتبر")]
        [MaxLength(9,ErrorMessage ="شماره دانشجویی نامعتبر")]
        public string StudentCode { get; set; }
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [MinLength(10, ErrorMessage = "کد ملی نامعتبر")]
        [MaxLength(10, ErrorMessage = "کد ملی نامعتبر")]
        public string NationalCode { get; set; }
    }
}