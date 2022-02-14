using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_RoleList
    {
        public int ID { get; set; }

        [Display(Name = "نام لاتین")]
        public string Name { get; set; }

        [Display(Name = "نام فارسی")]
        public string Display { get; set; }

        [Display(Name = "سطح")]
        public int Level { get; set; }
    }
}