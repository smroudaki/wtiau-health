using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_StudentList
    {
        public int ID { get; set; }

        [Display(Name = "شماره دانشجویی")]
        public string Student_Code { get; set; }

        [Display(Name = "کد ملی")]
        public string Student_National { get; set; }

        [Display(Name = "نام")]
        public string Student_Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string Student_Family { get; set; }

        [Display(Name = "فرم یک")]
        public string Student_Form1 { get; set; }

        [Display(Name = "فرم دو")]
        public string Student_Form2 { get; set; }

        [Display(Name = "نوبت")]
        public string Student_Turn { get; set; }

        [Display(Name = "اطلاعات کاربری")]
        public string Student_Info { get; set; }

        [Display(Name = "اطلاعات سلامت")]
        public string Student_HealthInfo { get; set; }
    }
}