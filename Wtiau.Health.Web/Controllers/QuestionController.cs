using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;
using Wtiau.Health.Web.Models.Repository;
using System.Data.Entity;
using System.Net;

namespace Wtiau.Health.Web.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class QuestionController : Controller
    {
        HealthEntities db = new HealthEntities();

        public ActionResult Index(int id, string formDisplay)
        {
            var _Qusetion = db.Tbl_Question.Where(x => x.Question_IsDelete == false && x.Tbl_FormStep.FS_FormID == id).Select(x => new Model_QusetionList
            {
                ID = x.Question_ID,
                Name = x.Question_Title,
                type = x.Question_TypeCodeID,
                Lie = x.Question_Lie,
                step = x.Tbl_FormStep.FS_Display,
                ResponseCount = x.Tbl_Response.Where(xx => xx.Response_IsDelete == false).ToList().Count,
            }).ToList();

            ViewBag.FormId = id;
            ViewBag.FormDisplay = formDisplay;

            return View(_Qusetion);
        }

        [HttpGet]
        public ActionResult QuestionCreate(int id)
        {
            Model_QuestionAdd model = new Model_QuestionAdd()
            {
                ID = id
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult QuestionCreate(Model_QuestionAdd model)
        {
            Tbl_Question _Question = new Tbl_Question()
            {
                Question_Title = model.Title,
                Question_Guid = Guid.NewGuid(),
                Question_FormID = model.ID,
                Question_Lie = model.Lie,
                Question_FSID = db.Tbl_FormStep.Where(a => a.FS_Guid.ToString() == model.Step).SingleOrDefault().FS_ID,
            };

            _Question.Tbl_Code = db.Tbl_Code.Where(x => x.Code_Guid.ToString() == model.Type).SingleOrDefault();

            db.Tbl_Question.Add(_Question);

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

        public ActionResult QuestionEdit(int id)
        {
            var q = db.Tbl_Question.Where(x => x.Question_ID == id).SingleOrDefault();

            if (q != null)
            {
                Model_QuestionEdit model = new Model_QuestionEdit()
                {
                    ID = q.Question_ID,
                    Title = q.Question_Title,
                    Step = q.Question_FSID.ToString(),
                    Form_ID = q.Question_FormID
                    //type = 
                };

                return PartialView(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult QuestionEdit(Model_QuestionEdit model)
        {
            if (ModelState.IsValid)
            {
                Tbl_Question q = db.Tbl_Question.Where(x => x.Question_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.Question_Title = model.Title;
                    q.Question_TypeCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(Guid.Parse(model.Type));
                    q.Tbl_FormStep.FS_ID = Convert.ToInt32(model.Step);

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

                        return RedirectToAction("Index", new { id = model.ID });
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult QuestionDelete(int? id)
        {
            if (id != null)
            {
                Model_MessageModal model = new Model_MessageModal();

                var q = db.Tbl_Question.Where(x => x.Question_ID == id).SingleOrDefault();

                if (q != null)
                {
                    model.ID = id.Value;
                    model.Name = q.Question_Title;
                    model.Description = "آیا از حذف سوال مورد نظر اطمینان دارید ؟";

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
        public ActionResult QuestionDelete(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var q = db.Tbl_Question.Where(x => x.Question_ID == model.ID).SingleOrDefault();

                if (q != null)
                {
                    q.Question_IsDelete = true;

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

        public JsonResult Get_StepList(string searchTerm, int id)
        {
            var q = db.Tbl_FormStep.Where(x => x.FS_IsDelete == false && x.FS_FormID == id).ToList();

            if (searchTerm != null)
            {
                q = q.Where(a => a.FS_Display.Contains(searchTerm)).ToList();
            }

            var md = q.Select(a => new { id = a.FS_Guid.ToString(), text = a.FS_Display });

            return Json(md, JsonRequestBehavior.AllowGet);
        }
    }
}