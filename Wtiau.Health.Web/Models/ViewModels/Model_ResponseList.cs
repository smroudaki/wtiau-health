using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_ResponseList
    {
        public int ID { get; set; }

        [Display(Name = "ترتیب")]
        public int Order { get; set; }

        [Display(Name = "عنوان")]
        [DataType(DataType.MultilineText)]
        public string Title { get; set; }

        [Display(Name = "امتیاز")]
        public int Hint { get; set; }

        [Display(Name = "پاسخ صحیح")]
        public bool IsTrue { get; set; }
    }
}