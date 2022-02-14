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

namespace Wtiau.Health.Web.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ResponseController : Controller
    {
        HealthEntities db = new HealthEntities();

        public ActionResult Index(int id, int formId, string formDisplay, string questionDisplay)
        {
            var _Response = db.Tbl_Response.Where(x => x.Tbl_Question.Question_ID == id).Select(x => new Model_ResponseList
            {
                ID = x.Response_ID,
                Order = x.Response_Order,
                Title = x.Response_Title,
                Hint = x.Response_Hint,
                IsTrue = x.Response_IsTrue,
            }).ToList();

            ViewBag.FormId = formId;
            ViewBag.FormDisplay = formDisplay;
            ViewBag.QuestionID = id;
            ViewBag.QuestionDisplay = questionDisplay;

            return View(_Response);
        }

        [HttpGet]
        public ActionResult ResponseAdd(int id)
        {
            Model_ResponseAdd model = new Model_ResponseAdd()
            {
                ID = id
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ResponseAdd(Model_ResponseAdd model)
        {
            Tbl_Response _Response = new Tbl_Response()
            {
                Response_Guid = Guid.NewGuid(),
                Response_QuestionID = model.ID,
                Response_Title = model.Title,
                Response_Order = model.Order,
                Response_IsTrue = model.IsTrue,
                Response_Hint = model.Hint,
            };

            db.Tbl_Response.Add(_Response);

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
        public ActionResult ResponseEdit(int id)
        {
            Model_ResponseEdit model = db.Tbl_Response.Where(x => x.Response_IsDelete == false && x.Response_ID == id).Select(x => new Model_ResponseEdit
            {
                ID = id,
                Hint = x.Response_Hint,
                IsTrue = x.Response_IsTrue,
                Order = x.Response_Order,
                Title = x.Response_Title

            }).SingleOrDefault();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ResponseEdit(Model_ResponseEdit model)
        {
            var resp = db.Tbl_Response.Where(a => a.Response_ID == model.ID).SingleOrDefault();

            resp.Response_Hint = model.Hint;
            resp.Response_IsTrue = model.IsTrue;
            resp.Response_Order = model.Order;
            resp.Response_Title = model.Title;


            db.Entry(resp).State = EntityState.Modified;

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                return RedirectToAction("Index", new { id = resp.Response_QuestionID , formId = resp.Tbl_Question.Tbl_Form.Form_ID , formDisplay = resp.Tbl_Question.Tbl_Form.Form_Name, questionDisplay = resp.Tbl_Question.Question_Title });
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "خطا";

                return RedirectToAction("Index", new { id = resp.Response_QuestionID, formId = resp.Tbl_Question.Tbl_Form.Form_ID, formDisplay = resp.Tbl_Question.Tbl_Form.Form_Name, questionDisplay = resp.Tbl_Question.Question_Title });
            }
        }
    }
}