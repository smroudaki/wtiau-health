using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_TurnAdd
    {
        public int ID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Titel { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [DataType(DataType.MultilineText)]
        public string Descripton { get; set; }

        [Display(Name = " ")]
        public bool Activeness { get; set; }
    }
}