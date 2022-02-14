using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_FormList
    {
        public int ID { get; set; }

        [Display(Name = "نام فرم")]
        public string Form_Name { get; set; }

        [Display(Name = "تعداد سوال")]
        public int Form_QuestionCount { get; set; }

        [Display(Name = "تعداد مراحل")]
        public int Form_StepCount { get; set; }

        [Display(Name = "وضعیت")]
        public bool Form_IsActive { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string Form_CreateDate { get; set; }

    }
}