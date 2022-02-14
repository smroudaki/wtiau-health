using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_TimeSheetList
    {
        public int ID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string name { get; set; }
               
        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int MaxSize { get; set; }

        [Display(Name = "پر شده")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public int Size { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public bool Activeness { get; set; }
    }
}