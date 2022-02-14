using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_UserEdit
    {
        public int ID { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string Family { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(11, ErrorMessage = "مقدار وارد شده بیش 11 کارکتراست")]
        public string Mobile { get; set; }

        [Display(Name = "نقش")]
        public int Role { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(200, ErrorMessage = "مقدار وارد شده بیش 200 کارکتراست")]
        [EmailAddress(ErrorMessage = "ایمیل را به درستی وارد نمایید")]
        public string Email { get; set; }
    }
}