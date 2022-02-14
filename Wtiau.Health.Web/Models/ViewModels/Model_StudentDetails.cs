using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_StudentDetails
    {
        public int ID { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Family { get; set; }

        [Display(Name = "شماره دانشجویی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string StudentCode { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string NationalCode { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [MinLength(11, ErrorMessage = "شماره موبایل نامعتبر")]
        [MaxLength(11, ErrorMessage = "شماره موبایل نامعتبر")]
        public string Mobile { get; set; }

        [Display(Name = "تلفن ثابت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [MinLength(8, ErrorMessage = "شماره موبایل نامعتبر")]
        public string Phone { get; set; }

        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Gender { get; set; }

        [Display(Name = "سال تولد")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string BirthYear { get; set; }

        [Display(Name = "ملیت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string National { get; set; }

        [Display(Name = "گروه خونی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Blood { get; set; }

        [Display(Name = "بیمه")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Insurance { get; set; }

        [Display(Name = "مقطع تحصیلی پذیرفته شده در دانشگاه")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Grade { get; set; }

        [Display(Name = "محل سکونت دوره دانشجویی")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string HomeType { get; set; }

        [Display(Name = "وضیعیت تاهل")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Marriage { get; set; }

        [Display(Name = "دانشگاه")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string University { get; set; }

        [Display(Name = "دانشکده")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string College { get; set; }

        [Display(Name = "رشته")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Branch { get; set; }

        [Display(Name = "محل تولد")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string BirthLocation { get; set; }

        [Display(Name = "مقطع و دانشگاه قبلی")]
        public string BeforeUniversity { get; set; }




        [Display(Name = "قد (سانتی متر)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double Height { get; set; }

        [Display(Name = "وزن (کیلوگرم)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double Weight { get; set; }

        [Display(Name = "قند خون")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double BloodSuger { get; set; }

        [Display(Name = "فشار خون Min")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double BloodPressureMin { get; set; }

        [Display(Name = "فشار خون Max")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double BloodPressureMax { get; set; }

        [Display(Name = "BMI")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public double BMI { get; set; }



        [Display(Name = "فرم (ها)")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public List<Model_StudentDetailsForms> Forms { get; set; } = new List<Model_StudentDetailsForms>();
    }

    public class Model_StudentDetailsForms
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Name { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Display { get; set; }

        [Display(Name = "سوالات")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public List<Model_StudentDetailsFormsQuestions> Questions { get; set; } = new List<Model_StudentDetailsFormsQuestions>();
    }

    public class Model_StudentDetailsFormsQuestions
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Name { get; set; }

        [Display(Name = "سوالات")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public List<string> Responses { get; set; } = new List<string>();
    }
}