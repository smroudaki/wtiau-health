using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_FormStepList
    {
        public int ID { get; set; }

        [Display(Name = "عنوان لاتین")]
        public string Name { get; set; }

        [Display(Name = "عنوان")]
        public string Display { get; set; }

        [Display(Name = "تعداد سوال")]
        public int QuestionCount { get; set; }

        [Display(Name = "ترتیب")]
        public int Order { get; set; }
    }
}