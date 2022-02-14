using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Controllers
{
    public class PortalTakeTurnController : Controller
    {
        HealthEntities db = new HealthEntities();


        [HttpGet]
        public ActionResult Index()
        {
            var q = db.Tbl_Student.Where(a => a.Student_Code == User.Identity.Name).SingleOrDefault();

            if (q != null)
            {
                if (q.Student_TakeTurn)
                {
                    return RedirectToAction("index", "Portal");
                }
                else
                {
                    return View();
                }


            }
            return RedirectToAction("index", "Portal");

        }


        [HttpPost]
        public ActionResult Index(Model_TakeTurn model)
        {
            int ID = Convert.ToInt32(model.Sheft);



            Tbl_TurnTimeSheet _TurnTimeSheet = db.Tbl_TurnTimeSheet.Where(a => a.TTS_ID == ID).SingleOrDefault();
            Tbl_Student _Student = db.Tbl_Student.Where(a => a.Student_Code == User.Identity.Name).SingleOrDefault();


            if (_Student.Student_TakeTurn)
            {
                return RedirectToAction("index", "Portal");
            }


            if (_TurnTimeSheet.TTS_MaxSize > _TurnTimeSheet.Tbl_TakeTurn.Count())
            {
                Tbl_TakeTurn _TakeTurn = new Tbl_TakeTurn()
                {
                    TT_Guid = Guid.NewGuid(),
                    TT_StudentID = _Student.Student_ID,
                    TT_TTSID = _TurnTimeSheet.TTS_ID,
                };

                db.Tbl_TakeTurn.Add(_TakeTurn);

                _Student.Student_TakeTurn = true;

                db.Entry(_Student).State = EntityState.Modified;


                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                    return RedirectToAction("index", "Portal");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "خطا";

                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ظرفیت این تایم تکمیل شده است";

                return RedirectToAction("Index");
            }

        }


        [HttpPost]
        public JsonResult Get_ShiftList(int TurnID)
        {
            var q = db.Tbl_Turn.Where(a => a.Turn_ID == TurnID).SingleOrDefault();

            var t = db.Tbl_TurnTimeSheet.Where(a => a.TTS_TurnID == q.Turn_ID).ToList();

            List<Model_DropDown> list = new List<Model_DropDown>();

            foreach (var item in t)
            {
                if (item.TTS_MaxSize > item.Tbl_TakeTurn.Count())
                {
                    list.Add(new Model_DropDown(item.TTS_ID, item.TTS_Name));
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }


    }
}