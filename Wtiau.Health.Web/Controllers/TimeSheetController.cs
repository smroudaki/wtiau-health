using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Controllers
{
    public class TimeSheetController : Controller
    {
        HealthEntities db = new HealthEntities();

        [HttpGet]
        public ActionResult Index(int id, string turnDisplay)
        {
            var _timesheet = db.Tbl_TurnTimeSheet.Where(x => x.TTS_IsDelete == false && x.TTS_TurnID == id ).Select(x => new Model_TimeSheetList
            {
                ID = x.TTS_ID,
                name = x.TTS_Name,
                MaxSize = x.TTS_MaxSize,
                Activeness = x.TTS_IsActive,
                Size = x.Tbl_TakeTurn.Where(a => a.TT_IsDelete == false).Count(),

            }).ToList();

            ViewBag.ID = id;
            ViewBag.TurnDisplay = turnDisplay;

            return View(_timesheet);
        }

        [HttpGet]
        public ActionResult TimeSheetAdd(int id)
        {
            Model_TimeSheetAdd model = new Model_TimeSheetAdd()
            {
                ID = id,
            };

            return PartialView(model);

        }

        [HttpPost]
        public ActionResult TimeSheetAdd(Model_TimeSheetAdd model)
        {
            Tbl_TurnTimeSheet _timesheet = new Tbl_TurnTimeSheet()
            {
                TTS_Name = model.name,
                TTS_MaxSize = model.MaxSize,
                TTS_Guid = Guid.NewGuid(),
                TTS_TurnID = model.ID,

            };

            db.Tbl_TurnTimeSheet.Add(_timesheet);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                return RedirectToAction("Index", new { id = model.ID });

            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "خطا";

                return RedirectToAction("Index", new { id = model.ID });


            }

        }

        [HttpGet]
        public ActionResult TimeSheetDelete(int? id)
        {
            if (id.HasValue)
            {
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_Turn.Where(x => x.Turn_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.Turn_Name;
                    model.Description = "آیا از حذف شیفت مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult TimeSheetDelete(Model_MessageModal model)
        {

            if (ModelState.IsValid)
            {
                var q = db.Tbl_TurnTimeSheet.Where(x => x.TTS_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.TTS_IsDelete = true;

                    db.Entry(q).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                        return RedirectToAction("Index", new { id = model.ID });
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

        [HttpGet]
        public ActionResult TimeSheetEdit(int id)
        {
            Model_TimeSheetEdit model = db.Tbl_TurnTimeSheet.Where(a => a.TTS_ID == id).Select(a => new Model_TimeSheetEdit
            {
                ID = a.TTS_ID,
                Activeness = a.TTS_IsActive,
                MaxSize = a.TTS_MaxSize,
                name = a.TTS_Name,

            }).SingleOrDefault();


            return PartialView(model);
        }

        [HttpPost]
        public ActionResult TimeSheetEdit(Model_TimeSheetEdit model)
        {
            Tbl_TurnTimeSheet _timesheet = db.Tbl_TurnTimeSheet.Where(a => a.TTS_ID == model.ID).SingleOrDefault();

            if (_timesheet != null)
            {
                _timesheet.TTS_Name = model.name;
                _timesheet.TTS_IsActive = model.Activeness;
                _timesheet.TTS_MaxSize = model.MaxSize;

                db.Entry(_timesheet).State = EntityState.Modified;

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";


                    return RedirectToAction("Index", new { id = _timesheet.TTS_TurnID });
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "خطا";


                    return RedirectToAction("Index", new { id = _timesheet.TTS_TurnID });

                }
            }
            return View();
        }
    }
}