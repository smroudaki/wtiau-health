using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_TakeTurn
    {
        [Display(Name = "روز")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Turn  { get; set; }

        [Display(Name = "نوبت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Sheft { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Discription { get; set; }
    }
}