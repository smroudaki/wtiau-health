using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_StudentCreate
    {
        [Display(Name = "شماره دانشجویی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [MaxLength(9, ErrorMessage = "مقدار وارد شده بیش از حد مجاز است")]
        [MinLength(9, ErrorMessage = "مقدار وارد شده کمتر از حد مجاز است")]
        public string Student_Code { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [MaxLength(10, ErrorMessage = "مقدار وارد شده بیش از حد مجاز است")]
        [MinLength(10, ErrorMessage = "مقدار وارد شده کمتر از حد مجاز است")]
        public string Student_NationalCode { get; set; }
    }
}