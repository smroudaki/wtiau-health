using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_FormEdit
    {
        public int ID { get; set; }

        [Display(Name = "نام لاتین")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Form_Display { get; set; }

        [Display(Name = "نام فرم")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Form_Name { get; set; }

        [Display(Name = "دوره")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Course { get; set; }
    }
}