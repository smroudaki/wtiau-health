using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_FormCreate
    {
        public int ID { get; set; }
        [Display(Name = "نام لاتین")]
        public string Form_Display { get; set; }
        [Display(Name = "نام فرم")]
        public string Form_Name { get; set; }
        [Display(Name = "دوره")]
        public string Course { get; set; }

    }
}