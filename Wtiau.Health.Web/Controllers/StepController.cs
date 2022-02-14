using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class StepController : Controller
    {
        HealthEntities db = new HealthEntities();

        public ActionResult Index(int id, string formDisplay)
        {
            var _step = db.Tbl_FormStep.Where(x => x.FS_IsDelete == false && x.FS_FormID == id).Select(x => new Model_FormStepList
            {
                ID = x.FS_ID,
                Name = x.FS_Name,
                Display = x.FS_Display,
                QuestionCount = db.Tbl_FormStep.Where(xx => xx.FS_FormID == id).FirstOrDefault().Tbl_Question.Where(xx => xx.Question_IsDelete == false).ToList().Count,
                Order = x.FS_Order
            }).ToList();

            ViewBag.FormDisplay = formDisplay;
            ViewBag.ID = id;

            return View(_step);
        }

    }
}