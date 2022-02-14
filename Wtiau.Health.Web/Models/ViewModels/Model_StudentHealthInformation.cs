using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_StudentHealthInformation
    {
        public int ID { get; set; }

        [Display(Name = "قد (سانتی متر)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double Height { get; set; }

        [Display(Name = "وزن (کیلوگرم)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double Weight { get; set; }

        [Display(Name = "قند خون")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double BloodSuger { get; set; }

        [Display(Name = "فشار خون Min")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double BloodPressureMin { get; set; }

        [Display(Name = "فشار خون Max")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double BloodPressureMax { get; set; }
    }
}