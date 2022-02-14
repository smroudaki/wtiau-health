﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wtiau.Health.Web.Models.ViewModels
{
    public class Model_StudentInfo
    {
        public int ID { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Family { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [MinLength(11, ErrorMessage = "شماره موبایل نامعتبر")]
        [MaxLength(11, ErrorMessage = "شماره موبایل نامعتبر")]
        public string Mobile { get; set; }

        [Display(Name = "تلفن ثابت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [MinLength(8, ErrorMessage = "شماره موبایل نامعتبر")]
        public string Phone { get; set; }

        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Gender { get; set; }

        [Display(Name = "سال تولد")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string BirthYear { get; set; }

        [Display(Name = "ملیت")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string National { get; set; }

        [Display(Name = "گروه خونی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Blood { get; set; }

        [Display(Name = "بیمه")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Insurance { get; set; }

        [Display(Name = "مقطع تحصیلی پذیرفته شده در دانشگاه")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Grad { get; set; }

        [Display(Name = "محل سکونت دوره دانشجویی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string HomeType { get; set; }

        [Display(Name = "وضیعیت تاهل")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Marriage { get; set; }

        [Display(Name = "دانشگاه")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string University { get; set; }

        [Display(Name = "دانشکده")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string College { get; set; }

        [Display(Name = "رشته")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string Branch { get; set; }

        [Display(Name = "محل تولد")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public string BirthLocation { get; set; }

        [Display(Name = "در صورتی که مقطع تحصیلی دانشگاهی قبلی داشته اید؟ ذکر نمایید")]
        public string BeforeUniversity { get; set; }
    }
}