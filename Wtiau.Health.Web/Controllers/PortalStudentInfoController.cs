using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Controllers
{
    public class PortalStudentInfoController : Controller
    {
        HealthEntities db = new HealthEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var q = db.Tbl_Student.Where(a => a.Student_Code == User.Identity.Name).SingleOrDefault();
            if (q != null)
            {
                if (!q.Student_Info)
                {
                    return View();
                }
                else if (!q.Student_Form1)
                {
                    return RedirectToAction("ShowForm", "PortalForms", new { ID = "c78afdf3-a647-4e54-95e8-94869576b7e5" });
                }
                else if (!q.Student_Form2)
                {
                    return RedirectToAction("ShowForm", "PortalForms", new { ID = "de54c8ba-6032-4dc5-9241-2e6614a9840d" });
                }
                else if (!q.Student_TakeTurn)
                {
                    return RedirectToAction("index", "PortalTakeTurn");
                }
                else
                {
                    return RedirectToAction("index", "Portal");
                }
            }
            else
            {
                return RedirectToAction("index", "Portal");
            }

        }


        [HttpPost]
        public ActionResult Index(Model_StudentInfo model)
        {
            Tbl_Student _Student = db.Tbl_Student.Where(a => a.Student_Code == User.Identity.Name).SingleOrDefault();

            if (_Student.Student_SIID != null || _Student.Student_Info)
            {
                return RedirectToAction("ShowForm", "PortalForms", new { ID = "c78afdf3-a647-4e54-95e8-94869576b7e5" });
            }

            Tbl_StudentInfo _info = new Tbl_StudentInfo()
            {
                SI_Name = model.Name,
                SI_Family = model.Family,
                SI_Phone = model.Phone,
                SI_Mobile = model.Mobile,
                SI_Email = model.Email,
                SI_BirthdayLocation = model.BirthLocation,
                SI_BeforeUniversity = model.BeforeUniversity,
                SI_Guid = Guid.NewGuid(),
                SI_InsuranceCodeID = Convert.ToInt32(model.Insurance),
                SI_BloodCodeID = Convert.ToInt32(model.Blood),
                SI_GenderCodeID = Convert.ToInt32(model.Gender),
                SI_NationalCodeID = Convert.ToInt32(model.National),
                SI_HomeTypeCodeID = Convert.ToInt32(model.HomeType),
                SI_MarriageCodeID = Convert.ToInt32(model.Marriage),
                SI_BranchID = Convert.ToInt32(model.Branch),
                SI_BirthYearCodeID = Convert.ToInt32(model.BirthYear),
            };

            db.Tbl_StudentInfo.Add(_info);


            _Student.Tbl_StudentInfo = _info;
            _Student.Student_Info = true;

            db.Entry(_Student).State = EntityState.Modified;

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                return RedirectToAction("ShowForm", "PortalForms", new { ID = "c78afdf3-a647-4e54-95e8-94869576b7e5" });
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "خطا";

                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public JsonResult Get_CollageList(int UnivercityID)
        {
            var q = db.Tbl_University.Where(a => a.University_ID == UnivercityID).SingleOrDefault();

            var t = q.Tbl_College.ToList();

            var md = t.Select(a => new { id = a.College_ID, text = a.College_Display });

            return Json(md, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_GradeList(int CollageID)
        {
            var q = db.Tbl_College.Where(a => a.College_ID == CollageID).SingleOrDefault();

            var t = q.Tbl_Grad.ToList();

            var md = t.Select(a => new { id = a.Grade_ID, text = a.Grade_Display });

            return Json(md, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_BranchList(int GradeID)
        {
            var q = db.Tbl_Grad.Where(a => a.Grade_ID == GradeID).SingleOrDefault();

            var t = q.Tbl_Branch.ToList();

            var md = t.Select(a => new { id = a.Branch_ID, text = a.Branch_Display });

            return Json(md, JsonRequestBehavior.AllowGet);
        }


    }
}