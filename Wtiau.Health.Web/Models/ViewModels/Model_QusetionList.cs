using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_QusetionList
    {
        public int ID { get; set; }
        [Display(Name = "عنوان سوال")]
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }
        [Display(Name = "نوع سوال")]
        public int type { get; set; }
        [Display(Name = "مرحله")]
        public string step { get; set; }
        [Display(Name = "دروغ سنج")]
        public bool Lie { get; set; }
        [Display(Name = "تعداد گزینه ها")]
        public int ResponseCount { get; set; }

    }
}