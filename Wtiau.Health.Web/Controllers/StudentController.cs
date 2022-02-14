using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.Plugins;
using Wtiau.Health.Web.Models.Repository;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin, DataEntry")]
    public class StudentController : Controller
    {
        HealthEntities db = new HealthEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var _student = db.Tbl_Student.Where(x => x.Student_IsDelete == false).Select(x => new Model_StudentList
            {
                ID = x.Student_ID,
                Student_Code = x.Student_Code,
                Student_National = x.Student_NationalCode,
                Student_Form1 = x.Student_Form1 ? "تکمیل" : "ثبت نشده",
                Student_Form2 = x.Student_Form2 ? "تکمیل" : "ثبت نشده",
                Student_Turn = x.Student_TakeTurn ? "تکمیل" : "ثبت نشده",
                Student_Name = x.Student_SIID.HasValue ? (x.Tbl_StudentInfo.SI_IsDelete ? "ثبت نشده" : x.Tbl_StudentInfo.SI_Name) : "نا معلوم",
                Student_Family = x.Student_SIID.HasValue ? (x.Tbl_StudentInfo.SI_IsDelete ? "ثبت نشده" : x.Tbl_StudentInfo.SI_Family) : "نا معلوم",
                Student_Info = x.Student_Info ? "تکمیل" : "ثبت نشده",
                Student_HealthInfo = x.Student_HealthInfo ? "تکمیل" : "ثبت نشده"

            }).ToList();

            return View(_student);
        }

        [HttpPost]
        public ActionResult GetStudents()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            var _student = db.Tbl_Student.Where(x => x.Student_IsDelete == false).Select(x => new Model_StudentList
            {
                ID = x.Student_ID,
                Student_Code = x.Student_Code,
                Student_National = x.Student_NationalCode,
                Student_Form1 = x.Student_Form1 ? "تکمیل" : "ثبت نشده",
                Student_Form2 = x.Student_Form2 ? "تکمیل" : "ثبت نشده",
                Student_Turn = x.Student_TakeTurn ? "تکمیل" : "ثبت نشده",
                Student_Name = x.Student_SIID.HasValue ? (x.Tbl_StudentInfo.SI_IsDelete ? "ثبت نشده" : x.Tbl_StudentInfo.SI_Name) : "نا معلوم",
                Student_Family = x.Student_SIID.HasValue ? (x.Tbl_StudentInfo.SI_IsDelete ? "ثبت نشده" : x.Tbl_StudentInfo.SI_Family) : "نا معلوم",
                Student_Info = x.Student_Info ? "تکمیل" : "ثبت نشده",
                Student_HealthInfo = x.Student_HealthInfo ? "تکمیل" : "ثبت نشده"

            }).ToList();

            int totalRows = _student.Count;

            if (!string.IsNullOrEmpty(searchValue))
            {
                _student = _student.Where(x => x.ID.ToString().ToLower().Contains(searchValue.ToLower()) ||
                                               x.Student_Code.ToLower().Contains(searchValue.ToLower()) ||
                                               x.Student_National.ToLower().Contains(searchValue.ToLower()) ||
                                               x.Student_Form1.ToString().ToLower().Contains(searchValue.ToLower()) ||
                                               x.Student_Form2.ToString().ToLower().Contains(searchValue.ToLower()) ||
                                               x.Student_Turn.ToString().ToLower().Contains(searchValue.ToLower()) ||
                                               (string.IsNullOrEmpty(x.Student_Name) ? false : x.Student_Name.ToLower().Contains(searchValue.ToLower())) ||
                                               (string.IsNullOrEmpty(x.Student_Family) ? false : x.Student_Family.ToLower().Contains(searchValue.ToLower())) ||
                                               x.Student_Info.ToString().ToLower().Contains(searchValue.ToLower()) ||
                                               x.Student_HealthInfo.ToString().ToLower().Contains(searchValue.ToLower())).ToList();
            }

            int totalRowsAfterFiltering = _student.Count;

            _student = _student.OrderBy(sortColumnName + " " + sortDirection).ToList();

            _student = _student.Skip(start).Take(length).ToList();

            return Json(new { data = _student, draw = Request["draw"], recordsTotal = totalRows, recordsFiltered = totalRowsAfterFiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model_StudentCreate model)
        {
            if (ModelState.IsValid)
            {
                if (db.Tbl_Student.Where(x => x.Student_Code == model.Student_Code).Any())
                {
                    TempData["TosterState"] = "info";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "اطلاعات دانشجوی مورد نظر قبلا در سامانه ثبت شده است.";

                    return RedirectToAction("Index");
                }

                Tbl_Student Student = new Tbl_Student()
                {
                    Student_Guid = Guid.NewGuid(),
                    Student_Code = model.Student_Code,
                    Student_NationalCode = model.Student_NationalCode
                };

                db.Tbl_Student.Add(Student);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "ثبت شد";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "ثبت نشد";

                    return RedirectToAction("Index");
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                var _Student = db.Tbl_Student.Where(x => x.Student_ID == id && !x.Student_IsDelete).SingleOrDefault();

                if (_Student != null)
                {
                    Model_StudentDetails model = new Model_StudentDetails();

                    var _StudentInfo = db.Tbl_StudentInfo.Where(x => x.SI_ID == _Student.Student_SIID && !x.SI_IsDelete).SingleOrDefault();
                    var _Forms = db.Tbl_Form.Where(x => x.Form_IsActive && !x.Form_IsDelete).ToList();
                    var _StudentHealthInformation = db.Tbl_StudentHealthInformation.Where(x => x.SHI_StudentID == _Student.Student_ID && !x.SHI_IsDelete).SingleOrDefault();

                    if (_StudentInfo != null)
                    {
                        model.ID = _Student.Student_ID;
                        model.Name = _StudentInfo.SI_Name;
                        model.Family = _StudentInfo.SI_Family;
                        model.Email = _StudentInfo.SI_Email;
                        model.StudentCode = _Student.Student_Code;
                        model.NationalCode = _Student.Student_NationalCode;
                        model.Mobile = _StudentInfo.SI_Mobile;
                        model.Phone = _StudentInfo.SI_Phone;
                        model.Gender = _StudentInfo.Tbl_Code.Code_Display;
                        model.BirthYear = _StudentInfo.Tbl_Code1.Code_Display;
                        model.National = _StudentInfo.Tbl_Code2.Code_Display;
                        model.Blood = _StudentInfo.Tbl_Code3.Code_Display;
                        model.Insurance = _StudentInfo.Tbl_Code4.Code_Display;
                        model.HomeType = _StudentInfo.Tbl_Code5.Code_Display;
                        model.Marriage = _StudentInfo.Tbl_Code6.Code_Display;
                        model.University = _StudentInfo.Tbl_Branch.Tbl_Grad.Tbl_College.Tbl_University.University_Display;
                        model.College = _StudentInfo.Tbl_Branch.Tbl_Grad.Tbl_College.College_Display;
                        model.Branch = _StudentInfo.Tbl_Branch.Branch_Display;
                        model.Grade = _StudentInfo.Tbl_Branch.Tbl_Grad.Grade_Display;
                        model.BirthLocation = _StudentInfo.SI_BirthdayLocation;
                        model.BeforeUniversity = _StudentInfo.SI_BeforeUniversity;

                        ViewBag.StudentDisplay = _StudentInfo.SI_Name + " " + _StudentInfo.SI_Family;
                    }
                    else
                    {
                        model.ID = _Student.Student_ID;
                        model.StudentCode = _Student.Student_Code;
                        model.NationalCode = _Student.Student_NationalCode;

                        ViewBag.StudentDisplay = _Student.Student_Code;
                    }

                    if (_StudentHealthInformation != null)
                    {
                        model.Height = _StudentHealthInformation.SHI_Height;
                        model.Weight = _StudentHealthInformation.SHI_Weight;
                        model.BloodSuger = _StudentHealthInformation.SHI_BloodSuger;
                        model.BloodPressureMin = _StudentHealthInformation.SHI_BloodPressureMin;
                        model.BloodPressureMax = _StudentHealthInformation.SHI_BloodPressureMax;
                        model.BMI = _StudentHealthInformation.SHI_BMI;
                    }

                    if (_Forms != null)
                    {
                        foreach (var form in _Forms)
                        {
                            var _FormAnswers = db.Tbl_FormAnswer.Where(x => x.FA_StudentID == _Student.Student_ID && x.FA_FormID == form.Form_ID && !x.FA_IsDelete).ToList();

                            if (_FormAnswers != null)
                            {
                                Model_StudentDetailsForms model_StudentDetailsForms = new Model_StudentDetailsForms
                                {
                                    Name = form.Form_Name,
                                    Display = form.Form_Display
                                };

                                foreach (var item in _FormAnswers)
                                {
                                    var _FormAnswerResponse = item.Tbl_FormAnswerResponse.Where(x => !x.FAR_IsDelete).ToList();

                                    if (_FormAnswerResponse != null)
                                    {
                                        Model_StudentDetailsFormsQuestions model_StudentDetailsFormsQuestions = new Model_StudentDetailsFormsQuestions
                                        {
                                            Name = _FormAnswerResponse.First().Tbl_Response.Tbl_Question.Question_Title
                                        };

                                        foreach (var response in _FormAnswerResponse)
                                        {
                                            model_StudentDetailsFormsQuestions.Responses.Add(response.Tbl_Response.Response_Title);
                                        }

                                        model_StudentDetailsForms.Questions.Add(model_StudentDetailsFormsQuestions);
                                    }
                                };

                                model.Forms.Add(model_StudentDetailsForms);
                            }
                        }
                    }

                    return View(model);
                }
            }

            return HttpNotFound();
        }

        public ActionResult CreateHealthInformation(int? id)
        {
            if (id != null)
            {
                var _Student = db.Tbl_Student.Where(x => x.Student_ID == id).SingleOrDefault();

                if (_Student != null)
                {
                    return PartialView(new Model_StudentHealthInformation() { ID = id.Value });
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHealthInformation(Model_StudentHealthInformation model)
        {
            if (ModelState.IsValid)
            {
                var _Student = db.Tbl_Student.Where(x => x.Student_ID == model.ID).SingleOrDefault();

                if (_Student != null)
                {
                    if (_Student.Student_HealthInfo)
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.WithTitel;
                        TempData["TosterTitel"] = "خطا";
                        TempData["TosterMassage"] = "اطلاعات سلامت دانشجو مورد نظر قبلا در سامانه ثبت شده است.";

                        return RedirectToAction("Index");
                    }

                    Tbl_StudentHealthInformation _StudentHealthInformation = new Tbl_StudentHealthInformation()
                    {
                        SHI_Guid = Guid.NewGuid(),
                        SHI_StudentID = model.ID,
                        SHI_Height = model.Height,
                        SHI_Weight = model.Weight,
                        SHI_BloodSuger = model.BloodSuger,
                        SHI_BloodPressureMin = model.BloodPressureMin,
                        SHI_BloodPressureMax = model.BloodPressureMax,
                        SHI_BMI = Convertor.SetPrecision(model.Weight / Math.Pow(model.Height / 100, 2), 4)
                    };

                    _Student.Student_HealthInfo = true;

                    db.Tbl_StudentHealthInformation.Add(_StudentHealthInformation);
                    db.Entry(_Student).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت شد";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ثبت نشد";

                        return RedirectToAction("Index");
                    }
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult ChangeHealthInfo(int? id)
        {
            if (id != null)
            {
                var _StudentHealthInformation = db.Tbl_StudentHealthInformation.Where(x => x.Tbl_Student.Student_ID == id && !x.SHI_IsDelete).SingleOrDefault();

                if (_StudentHealthInformation != null)
                {
                    Model_StudentHealthInformation model = new Model_StudentHealthInformation()
                    {
                        ID = id.Value,
                        Height = _StudentHealthInformation.SHI_Height,
                        Weight = _StudentHealthInformation.SHI_Weight,
                        BloodSuger = _StudentHealthInformation.SHI_BloodSuger,
                        BloodPressureMin = _StudentHealthInformation.SHI_BloodPressureMin,
                        BloodPressureMax = _StudentHealthInformation.SHI_BloodPressureMax
                    };

                    return PartialView(model);
                }
                else
                {
                    return RedirectToAction("Details", new { id });
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeHealthInfo(Model_StudentHealthInformation model)
        {
            if (ModelState.IsValid)
            {
                var _StudentHealthInformation = db.Tbl_StudentHealthInformation.Where(x => x.Tbl_Student.Student_ID == model.ID && !x.SHI_IsDelete).SingleOrDefault();

                if (_StudentHealthInformation != null)
                {
                    _StudentHealthInformation.SHI_Height = model.Height;
                    _StudentHealthInformation.SHI_Weight = model.Weight;
                    _StudentHealthInformation.SHI_BloodSuger = model.BloodSuger;
                    _StudentHealthInformation.SHI_BloodPressureMin = model.BloodPressureMin;
                    _StudentHealthInformation.SHI_BloodPressureMax = model.BloodPressureMax;
                    _StudentHealthInformation.SHI_BMI = Convertor.SetPrecision(model.Weight / Math.Pow(model.Height / 100, 2), 4);

                    db.Entry(_StudentHealthInformation).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ویرایش شد";
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ویرایش نشد";
                    }

                    return RedirectToAction("Details", new { id = model.ID });
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult ChangeInfo(int? id)
        {
            if (id != null)
            {
                var _StudentInfo = db.Tbl_StudentInfo.Where(x => x.Tbl_Student.FirstOrDefault().Student_ID == id && !x.SI_IsDelete).SingleOrDefault();

                if (_StudentInfo != null)
                {
                    Model_StudentInfoEdit model = new Model_StudentInfoEdit()
                    {
                        ID = _StudentInfo.SI_ID,
                        Name = _StudentInfo.SI_Name,
                        Family = _StudentInfo.SI_Family,
                        Email = _StudentInfo.SI_Email,
                        Mobile = _StudentInfo.SI_Mobile,
                        Phone = _StudentInfo.SI_Phone,
                        Gender = _StudentInfo.SI_GenderCodeID,
                        BirthYear = _StudentInfo.SI_BirthYearCodeID,
                        National = _StudentInfo.SI_NationalCodeID,
                        Blood = _StudentInfo.SI_BloodCodeID,
                        Insurance = _StudentInfo.SI_InsuranceCodeID,
                        Grad = _StudentInfo.Tbl_Branch.Tbl_Grad.Grade_ID,//
                        HomeType = _StudentInfo.SI_HomeTypeCodeID,
                        Marriage = _StudentInfo.SI_MarriageCodeID,
                        University = _StudentInfo.Tbl_Branch.Tbl_Grad.Tbl_College.Tbl_University.University_ID,//
                        College = _StudentInfo.Tbl_Branch.Tbl_Grad.Tbl_College.College_ID,//
                        Branch = _StudentInfo.Tbl_Branch.Branch_ID,//
                        BirthLocation = _StudentInfo.SI_BirthdayLocation,
                        BeforeUniversity = _StudentInfo.SI_BeforeUniversity,
                    };

                    return PartialView(model);
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "اطلاعات هویتی دانشجوی مورد نظر در سامانه ثبت نشده است.";

                    return RedirectToAction("Details", new { id });
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeInfo(Model_StudentInfoEdit model)
        {
            if (ModelState.IsValid)
            {
                var _StudentInfo = db.Tbl_StudentInfo.Where(x => x.Tbl_Student.FirstOrDefault().Student_ID == model.ID && !x.SI_IsDelete).SingleOrDefault();

                if (_StudentInfo != null)
                {
                    _StudentInfo.SI_Name = model.Name;
                    _StudentInfo.SI_Family = model.Family;
                    _StudentInfo.SI_Mobile = model.Mobile;
                    _StudentInfo.SI_Email = model.Email;
                    _StudentInfo.SI_Phone = model.Phone;
                    _StudentInfo.SI_GenderCodeID = Convert.ToInt32(model.Gender);
                    _StudentInfo.SI_BirthYearCodeID = Convert.ToInt32(model.BirthYear);
                    _StudentInfo.SI_NationalCodeID = Convert.ToInt32(model.National);
                    _StudentInfo.SI_BloodCodeID = Convert.ToInt32(model.Blood);
                    _StudentInfo.SI_InsuranceCodeID = Convert.ToInt32(model.Insurance);
                    //Grad
                    _StudentInfo.SI_HomeTypeCodeID = Convert.ToInt32(model.HomeType);
                    _StudentInfo.SI_MarriageCodeID = Convert.ToInt32(model.Marriage);
                    //University
                    //College
                    _StudentInfo.SI_BranchID = Convert.ToInt32(model.Branch);//
                    _StudentInfo.SI_BirthdayLocation = model.BirthLocation;
                    _StudentInfo.SI_BeforeUniversity = model.BeforeUniversity;

                    db.Entry(_StudentInfo).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ویرایش شد";
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "ویرایش نشد";
                    }

                    return RedirectToAction("Details", new { id = model.ID });
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var _StudentInfo = db.Tbl_StudentInfo.Where(x => x.Tbl_Student.FirstOrDefault().Student_ID == id && !x.SI_IsDelete).SingleOrDefault();

                if (_StudentInfo != null)
                {
                    Model_MessageModal model = new Model_MessageModal
                    {
                        ID = id.Value,
                        Name = _StudentInfo.SI_Name + " " + _StudentInfo.SI_Family,
                        Description = "آیا از حذف دانشجوی مورد نظر اطمینان دارید ؟"
                    };

                    return PartialView(model);
                }
                else
                {
                    return RedirectToAction("Details", new { id });
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var _StudentInfo = db.Tbl_StudentInfo.Where(x => x.Tbl_Student.FirstOrDefault().Student_ID == model.ID && !x.SI_IsDelete).SingleOrDefault();

                if (_StudentInfo != null)
                {
                    _StudentInfo.SI_IsDelete = true;
                    _StudentInfo.Tbl_Student.FirstOrDefault().Student_Info = false;

                    db.Entry(_StudentInfo).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "دانشجوی مورد نظر با موفقیت حذف شد.";
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "دانشجوی مورد نظر با موفقیت حذف نشد.";
                    }

                    return RedirectToAction("Index");
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #region Data
        [HttpGet]
        public ActionResult ImportStudentFromExcel()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult ImportStudentFromExcel(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                }
                catch
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "خطا";
                    return View();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            Microsoft_Excel excel = new Microsoft_Excel(@"C:\2.xlsx");

            excel.Open(1);
            int row_count = excel.Get_RowCount();
            string[,] ex = excel.Get_Range(1, 1, row_count, 2);
            excel.Close();

            for (int i = 0; i < row_count; i++)
            {

                Tbl_Student _Student = new Tbl_Student()
                {
                    Student_Code = ex[i, 0],
                    Student_NationalCode = ex[i, 1],
                    Student_Guid = Guid.NewGuid()
                };

                db.Tbl_Student.Add(_Student);

            }


            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ثبت نام با موفقیت انجام شده";
                return View();
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "خطا";
                return View();
            }
        }

        public FileResult Excel_AllStudents()
        {
            string path = Path.Combine(Server.MapPath("~/Content/Reports/Excel/"), "StudentTemplate.xlsx");

            Microsoft_Excel _Excel = new Microsoft_Excel(path);
            _Excel.Open(1);

            int i = 1;

            foreach (var item in db.Tbl_Student.OrderBy(a => a.Student_Code).ToList())
            {
                _Excel.WriteToCell(i, 0, item.Student_Code);
                _Excel.WriteToCell(i, 1, item.Student_NationalCode);
                _Excel.WriteToCell(i, 2, item.Student_SIID.HasValue ? item.Tbl_StudentInfo.SI_Name : "نا معلوم");
                _Excel.WriteToCell(i, 3, item.Student_SIID.HasValue ? item.Tbl_StudentInfo.SI_Family : "نا معلوم");
                _Excel.WriteToCell(i, 4, item.Student_Info ? "تکمیل شده" : "تکمیل نشده");
                _Excel.WriteToCell(i, 5, item.Student_Form1 ? "تکمیل شده" : "تکمیل نشده");
                _Excel.WriteToCell(i, 6, item.Student_Form2 ? "تکمیل شده" : "تکمیل نشده");
                _Excel.WriteToCell(i, 7, item.Student_TakeTurn ? "تکمیل شده" : "تکمیل نشده");

                i++;
            }

            string SaveAsPath = Path.Combine(Server.MapPath("~/App_Data/Excel/"), string.Format("{0}.xlsx", Guid.NewGuid()));

            _Excel.SaveAs(SaveAsPath);

            _Excel.Close();

            return File(SaveAsPath, "*", string.Format("{0}.xlsx", "تمامی دانشجویان"));
        }

        public FileResult Excel_NotRegisterStudents()
        {
            string path = Path.Combine(Server.MapPath("~/Content/Reports/Excel/"), "StudentTemplate.xlsx");

            Microsoft_Excel _Excel = new Microsoft_Excel(path);
            _Excel.Open(1);

            int i = 1;

            foreach (var item in db.Tbl_Student.Where(a => a.Student_Info == false || a.Student_Form1 == false || a.Student_Form2 == false || a.Student_TakeTurn == false).OrderBy(a => a.Student_Code).ToList())
            {
                _Excel.WriteToCell(i, 0, item.Student_Code);
                _Excel.WriteToCell(i, 1, item.Student_NationalCode);
                _Excel.WriteToCell(i, 2, item.Student_SIID.HasValue ? item.Tbl_StudentInfo.SI_Name : "نا معلوم");
                _Excel.WriteToCell(i, 3, item.Student_SIID.HasValue ? item.Tbl_StudentInfo.SI_Family : "نا معلوم");
                _Excel.WriteToCell(i, 4, item.Student_Info ? "تکمیل شده" : "تکمیل نشده");
                _Excel.WriteToCell(i, 5, item.Student_Form1 ? "تکمیل شده" : "تکمیل نشده");
                _Excel.WriteToCell(i, 6, item.Student_Form2 ? "تکمیل شده" : "تکمیل نشده");
                _Excel.WriteToCell(i, 7, item.Student_TakeTurn ? "تکمیل شده" : "تکمیل نشده");

                i++;
            }

            string SaveAsPath = Path.Combine(Server.MapPath("~/App_Data/Excel/"), string.Format("{0}.xlsx", Guid.NewGuid()));

            _Excel.SaveAs(SaveAsPath);

            _Excel.Close();

            return File(SaveAsPath, "*", string.Format("{0}.xlsx", "دانشجویان ثبت نام نشده"));
        }

        [HttpGet]
        public ActionResult Excel_Form()
        {
            return PartialView();
        }


        public FileResult Excel_Form(Model_FormExport model)
        {
            List<List<string>> studentResponse = new List<List<string>>();

            var form = db.Tbl_Form.Where(a => a.Form_ID == model.ID).SingleOrDefault();

            List<Tbl_Student> students = new List<Tbl_Student>();

            if (form.Form_Display == "Form1")
            {
                students = db.Tbl_Student.Where(a => a.Student_Form1 == true).OrderBy(a => a.Student_Code).ToList();

            }
            else
            {
                students = db.Tbl_Student.Where(a => a.Student_Form2 == true).OrderBy(a => a.Student_Code).ToList();

            }

            string path = Path.Combine(Server.MapPath("~/Content/Reports/Excel/"), "Empty.xlsx");
            Microsoft_Excel _Excel = new Microsoft_Excel(path);
            _Excel.Open(1);
            int i = 0;

            _Excel.WriteToCell(i, 0, "شماره دانشجویی");
            _Excel.WriteToCell(i, 1, "کدملی");
            _Excel.WriteToCell(i, 2, "نام");
            _Excel.WriteToCell(i, 3, "نام خانوادگی");

            int d = form.Tbl_Question.Where(a => a.Question_IsDelete == false).Count();

            for (int k = 1; k <= d; k++)
            {
                _Excel.WriteToCell(i, k + 3, k.ToString());
            }

            i++;
 

            foreach (var item in students)
            {

                _Excel.WriteToCell(i, 0, item.Student_Code);
                _Excel.WriteToCell(i, 1, item.Student_NationalCode);
                _Excel.WriteToCell(i, 2, item.Student_SIID.HasValue ? item.Tbl_StudentInfo.SI_Name : "نا معلوم");
                _Excel.WriteToCell(i, 3, item.Student_SIID.HasValue ? item.Tbl_StudentInfo.SI_Family : "نا معلوم");

                List<string> Response = new List<string>();

                var formQ = item.Tbl_FormAnswer.Where(a => a.FA_FormID == form.Form_ID).ToList();


                foreach (var q in formQ)
                {
                    string s = "";
                    var g = q.Tbl_FormAnswerResponse;
                    int x = g.Count;
                    foreach (var p in g)
                    {
                        s += p.Tbl_Response.Response_Title;
                        if (x > 1)
                        {
                            s += ",";
                        }
                        x--;
                    }


                    Response.Add(s);
                }

                for (int j = 0; j < Response.Count; j++)
                {

                    _Excel.WriteToCell(i, j + 4, Response[j]);
                }

                //studentResponse.Add(Response);


                i++;

            }



            string SaveAsPath = Path.Combine(Server.MapPath("~/App_Data/Excel/"), string.Format("{0}.xlsx", Guid.NewGuid()));

            _Excel.SaveAs(SaveAsPath);

            _Excel.Close();

            return File(SaveAsPath, "*", string.Format("{0}.xlsx", form.Form_Name));
        }

        #endregion
    }
}