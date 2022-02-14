using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_FormStepAdd
    {
        public int ID { get; set; }
        [Display(Name = "نام لاتین")]
        public string Name { get; set; }
        [Display(Name = "نام")]
        public string Display { get; set; }
        [Display(Name = "ترتیب")]
        public int Order { get; set; }
    }
}