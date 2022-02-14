using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;
using Wtiau.Health.Web.Models.Plugins;
using System.IO;

namespace Wtiau.Health.Web.Controllers
{
    public class StudentsTimeSheetController : Controller
    {
        HealthEntities db = new HealthEntities();

        // GET: StudentsTimeSheet
        public ActionResult Index(int id, int turnID, string turnDisplay, string timeSheetDisplay)
        {
            var _TakeTurn = db.Tbl_TurnTimeSheet.Where(a => a.TTS_ID == id).SingleOrDefault().Tbl_TakeTurn;

            var _Student = _TakeTurn.Select(a => a.Tbl_Student).Select(a => new Model_StudentTakeTimeList
            {
                ID = a.Student_ID,
                Student_Code = a.Student_Code,
                Student_National = a.Student_NationalCode,
                Student_Name = a.Student_SIID.HasValue ? a.Tbl_StudentInfo.SI_Name : "نا معلوم",
                Student_Family = a.Student_SIID.HasValue ? a.Tbl_StudentInfo.SI_Family : "نا معلوم",
                per = a.Tbl_TakeTurn.FirstOrDefault().TT_IsPresent,

            }).ToList();

            ViewBag.ID = id;
            ViewBag.turnID = turnID;
            ViewBag.TurnDisplay = turnDisplay;
            ViewBag.TimeSheetDisplay = timeSheetDisplay;

            return View(_Student);
        }

        public ActionResult SetActiveness(int id)
        {
            var _TakeTurn = db.Tbl_TakeTurn.Where(x => x.Tbl_Student.Student_ID == id).FirstOrDefault();

            if (_TakeTurn != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = _TakeTurn.TT_IsPresent
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult SetActiveness(Model_SetActiveness model)
        {
            if (ModelState.IsValid)
            {
                var _TakeTurn = db.Tbl_TakeTurn.Where(x => x.Tbl_Student.Student_ID == model.ID).FirstOrDefault();

                if (_TakeTurn != null)
                {
                    _TakeTurn.TT_IsPresent = model.Activeness;

                    db.Entry(_TakeTurn).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Index", new { id = _TakeTurn.Tbl_TurnTimeSheet.TTS_ID });
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public FileResult Excel (int id)
        {
            var _TakeTurn = db.Tbl_TurnTimeSheet.Where(a => a.TTS_ID == id).SingleOrDefault().Tbl_TakeTurn;

            var _Student = _TakeTurn.Select(a => a.Tbl_Student).OrderBy(a => a.Student_Code).Select(a => new Model_StudentTakeTimeList
            {
                ID = a.Student_ID,
                Student_Code = a.Student_Code,
                Student_National = a.Student_NationalCode,
                Student_Name = a.Student_SIID.HasValue ? a.Tbl_StudentInfo.SI_Name : "نا معلوم",
                Student_Family = a.Student_SIID.HasValue ? a.Tbl_StudentInfo.SI_Family : "نا معلوم",
                per = false,

            }).ToList();


            string path = Path.Combine(Server.MapPath("~/Content/Reports/Excel/"), "TurnReportTemplate.xlsx");

            Microsoft_Excel _Excel = new Microsoft_Excel(path);
            _Excel.Open(1);

            int i = 1;

            foreach (var item in _Student)
            {
                _Excel.WriteToCell(i, 0, item.Student_Code);
                _Excel.WriteToCell(i, 1, item.Student_National);
                _Excel.WriteToCell(i, 2, item.Student_Name);
                _Excel.WriteToCell(i, 3, item.Student_Family);

                i++;
            }

            string SaveAsPath = Path.Combine(Server.MapPath("~/App_Data/Excel/"), string.Format("{0}.xlsx",Guid.NewGuid()));

            _Excel.SaveAs(SaveAsPath);

            _Excel.Close();

            string filename = _TakeTurn.FirstOrDefault().Tbl_TurnTimeSheet.TTS_Name;

            return File(SaveAsPath, "*", string.Format("{0}.xlsx", filename));
        }
    }
}