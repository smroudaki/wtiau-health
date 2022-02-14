using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Controllers
{
    public class TurnController : Controller
    {
        HealthEntities db = new HealthEntities();

        // GET: Turn
        public ActionResult Index()
        {
            var _turn = db.Tbl_Turn.Where(x => x.Turn_IsDelete == false).Select(x => new Model_TurnList
            {
                Titel = x.Turn_Name,
                Descripton = x.Turn_Description,
                Activeness = x.Turn_IsActive,
                ID = x.Turn_ID,

            }).ToList();

            return View(_turn);
        }

        [HttpGet]
        public ActionResult TurnAdd()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult TurnAdd(Model_TurnAdd model)
        {
            Tbl_Turn _Turn = new Tbl_Turn()
            {
                Turn_Guid = Guid.NewGuid(),
                Turn_Name = model.Titel,
                Turn_Description = model.Descripton,
                Turn_IsActive = model.Activeness,
            };

            db.Tbl_Turn.Add(_Turn);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "خطا";

                return RedirectToAction("Index");

            }

        }

        [HttpGet]
        public ActionResult TurnEdit(int id)
        {
            Model_TurnEdit model = db.Tbl_Turn.Where(a => a.Turn_ID == id).Select(a => new Model_TurnEdit
            {
                ID = a.Turn_ID,
                Activeness = a.Turn_IsActive,
                Descripton = a.Turn_Description,
                Titel = a.Turn_Name,
            }).SingleOrDefault();


            return PartialView(model);
        }

        [HttpPost]
        public ActionResult TurnEdit(Model_TurnEdit model)
        {
            Tbl_Turn _Turn = db.Tbl_Turn.Where(a => a.Turn_ID == model.ID).SingleOrDefault();

            if (_Turn != null)
            {
                _Turn.Turn_Name = model.Titel;
                _Turn.Turn_IsActive = model.Activeness;
                _Turn.Turn_Description = model.Descripton;


                db.Entry(_Turn).State = EntityState.Modified;

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "خطا";

                    return RedirectToAction("Index");

                }
            }

            return View();
        }


        [HttpGet]
        public ActionResult TurnDelete(int? id)
        {
            if (id.HasValue)
            {
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_Turn.Where(x => x.Turn_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.Turn_Name;
                    model.Description = "آیا از حذف نوبت مورد نظر اطمینان دارید ؟";

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
        public ActionResult TurnDelete(Model_MessageModal model)
        {

            if (ModelState.IsValid)
            {
                var q = db.Tbl_Turn.Where(x => x.Turn_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.Turn_IsDelete = true;

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
    }
}