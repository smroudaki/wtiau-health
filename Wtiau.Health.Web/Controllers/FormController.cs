using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class FormController : Controller
    {
        HealthEntities db = new HealthEntities();

        public ActionResult Index()
        {
            var _Student = db.Tbl_Form.Where(x => x.Form_IsDelete == false).Select(x => new Model_FormList
            {
                ID = x.Form_ID,
                Form_Name = x.Form_Name,
                Form_QuestionCount = x.Tbl_Question.Where(xx => xx.Question_IsDelete == false).ToList().Count,
                Form_CreateDate = x.Form_CreateDate.ToString(),
                Form_StepCount = x.Tbl_FormStep.Where(xx => xx.FS_IsDelete == false).ToList().Count,
                Form_IsActive = x.Form_IsActive
            }).ToList();


            return View(_Student);
        }

        [HttpGet]
        public ActionResult CreateForm()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateForm(Model_FormCreate model)
        {
            Tbl_Form _Form = new Tbl_Form();
            _Form.Form_Name = model.Form_Name;
            _Form.Form_Display = model.Form_Display;
            _Form.Form_IsActive = true;
            _Form.Form_Guid = Guid.NewGuid();
            _Form.Form_IsDelete = false;
            _Form.Form_ModifyDate = DateTime.Now;
            _Form.Form_CreateDate = DateTime.Now;
            _Form.Tbl_Course = db.Tbl_Course.Where(a => a.Course_Guid.ToString() == model.Course.ToString()).SingleOrDefault();

            db.Tbl_Form.Add(_Form);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                return RedirectToAction("Index", "Form");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "خطا";

                return RedirectToAction("Index");
            }
        }

        public ActionResult EditForm(int id)
        {
            var q = db.Tbl_Form.Where(x => x.Form_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_FormEdit model = new Model_FormEdit()
                {
                    ID = q.Form_ID,
                    Form_Name = q.Form_Name,
                    Course = q.Tbl_Course.Course_Guid.ToString(),
                    Form_Display = q.Form_Display,
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditForm(Model_FormEdit model)
        {
            if (ModelState.IsValid)
            {
                Tbl_Form q = db.Tbl_Form.Where(x => x.Form_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.Form_Name = model.Form_Name;
                    q.Form_Display = model.Form_Display;
                    q.Form_CourseID = db.Tbl_Course.Where(x => x.Course_Guid.ToString() == model.Course).SingleOrDefault().Course_ID;

                    db.Entry(q).State = EntityState.Modified;

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
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult DeleteForm(int? id)
        {
            if (id != null)
            {
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_Form.Where(x => x.Form_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.Form_Name;
                    model.Description = "آیا از حذف پرسش نامه مورد نظر اطمینان دارید ؟";

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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_Form.Where(x => x.Form_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.Form_IsDelete = true;

                    db.Entry(q).State = EntityState.Modified;

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
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                        return HttpNotFound();
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult AddStep(int id)
        {
            Model_FormStepAdd model = new Model_FormStepAdd();

            model.ID = id;

            return PartialView();
        }

        [HttpPost]
        public ActionResult AddStep(Model_FormStepAdd model)
        {
            Tbl_FormStep _Step = new Tbl_FormStep()
            {
                FS_Display = model.Display,
                FS_Name = model.Name,
                FS_Order = model.Order,
                FS_FormID = model.ID,
                FS_Guid = Guid.NewGuid(),
                
            };

            db.Tbl_FormStep.Add(_Step);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                return RedirectToAction("Index", "Form");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "خطا";

                return RedirectToAction("Index");
            }
        }

        public ActionResult SetActiveness(int id)
        {
            var q = db.Tbl_Form.Where(x => x.Form_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_SetActiveness model = new Model_SetActiveness()
                {
                    ID = id,
                    Activeness = q.Form_IsActive
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetActiveness(Model_SetActiveness model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_Form.Where(x => x.Form_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.Form_IsActive = model.Activeness;

                    db.Entry(q).State = EntityState.Modified;

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
                        TempData["TosterMassage"] = "عملیات با موفقیت انجام نشده";

                        return RedirectToAction("Index");
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public JsonResult Get_CourseList(string searchTerm)
        {
            var q = db.Tbl_Course.Where(x => x.Course_IsDelete == false).ToList();

            if (searchTerm != null)
            {
                q = db.Tbl_Course.Where(a => a.Course_Years.Contains(searchTerm)).ToList();
            }

            var md = q.Select(a => new { id = a.Course_Guid, text = a.Course_Years });

            return Json(md, JsonRequestBehavior.AllowGet);
        }
    }
}