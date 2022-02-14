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
    public class PortalFormsController : Controller
    {
        HealthEntities db = new HealthEntities();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AlartForm()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult ShowForm(string ID)
        {
            var q = db.Tbl_Student.Where(a => a.Student_Code == User.Identity.Name).SingleOrDefault();
            if (q != null)
            {
                if (q.Student_Form1 && q.Student_Form2)
                {
                    if (!q.Student_Info)
                    {
                        return RedirectToAction("index", "PortalStudentInfo");
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
            }



            var F = db.Tbl_Form.Where(a => a.Form_IsDelete == false && a.Form_Guid.ToString() == ID).SingleOrDefault();

            if (F != null)
            {

                Model_Form _Form = new Model_Form()
                {
                    Name = F.Form_Name,
                    ID = F.Form_ID,
                };

                _Form.Steps = new List<Model_Steps>();

                List<Model_Steps> _Steps = new List<Model_Steps>();

                foreach (var S_item in F.Tbl_FormStep.Where(a => a.FS_IsDelete == false).ToList())
                {
                    Model_Steps _Step = new Model_Steps()
                    {
                        Name = S_item.FS_Display,
                    };

                    _Step.Questions = new List<Model_Questions>();

                    List<Model_Questions> _Questions = new List<Model_Questions>();

                    foreach (var Q_item in S_item.Tbl_Question.Where(a => a.Question_IsDelete == false).ToList())
                    {

                        Model_Questions _Question = new Model_Questions()
                        {
                            Titel = Q_item.Question_Title,
                            type = Q_item.Question_TypeCodeID,
                            Name = Q_item.Question_Guid.ToString(),
                        };

                        _Question.Responses = new List<Model_Responses>();

                        foreach (var R_item in Q_item.Tbl_Response.Where(a => a.Response_IsDelete == false).ToList())
                        {
                            _Question.Responses.Add(new Model_Responses()
                            {
                                Text = R_item.Response_Title,
                                Value = R_item.Response_Guid.ToString(),
                            });
                        }

                        _Step.Questions.Add(_Question);
                    }

                    _Form.Steps.Add(_Step);
                }

                return View(_Form);
            }
            else
            {

            }

            return View();
        }

        [HttpPost]
        public ActionResult ShowForm(FormCollection model)
        {
            var _student = db.Tbl_Student.Where(a => a.Student_Code == User.Identity.Name).SingleOrDefault();

            int? student_id = _student.Student_ID;

            if (student_id.HasValue)
            {
                Tbl_Form _form = new Tbl_Form();

                int i = 1;

                foreach (string key in model.AllKeys)
                {
                    if (key == "ID")
                    {
                        string q = model[key];
                        _form = db.Tbl_Form.Where(a => a.Form_IsDelete == false && a.Form_Guid.ToString() == q).SingleOrDefault();

                        if (_form.Form_Guid.ToString() == "c78afdf3-a647-4e54-95e8-94869576b7e5")
                        {
                            if (_student.Student_Form1)
                            {
                                return RedirectToAction("ShowForm", new { ID = "de54c8ba-6032-4dc5-9241-2e6614a9840d" });
                            }
                        }
                        else if (_form.Form_Guid.ToString() == "de54c8ba-6032-4dc5-9241-2e6614a9840d")
                        {
                            if (_student.Student_Form2)
                            {
                                return RedirectToAction("index", "PortalTakeTurn");
                            }
                        }
                    }
                    else
                    {
                        if (_form != null)
                        {
                            Guid guid;

                            if (Guid.TryParse(key, out guid))
                            {
                                string q = model[key];

                                Tbl_FormAnswer _answer = new Tbl_FormAnswer();
                                _answer.FA_Guid = Guid.NewGuid();
                                _answer.FA_FormID = _form.Form_ID;
                                _answer.FA_StudentID = student_id.Value;

                                db.Tbl_FormAnswer.Add(_answer);

                                string[] ANS = q.Split(',');

                                foreach (string item in ANS)
                                {
                                    Tbl_FormAnswerResponse _response = new Tbl_FormAnswerResponse();
                                    _response.FAR_Guid = Guid.NewGuid();
                                    _response.FAR_ResponseID = db.Tbl_Response.Where(a => a.Response_Guid.ToString() == item).SingleOrDefault().Response_ID;
                                    _response.Tbl_FormAnswer = _answer;

                                    db.Tbl_FormAnswerResponse.Add(_response);
                                }

                                i++;
                            }
                        }
                    }
                }

                i = 0;

                if (_form.Form_Guid.ToString() == "c78afdf3-a647-4e54-95e8-94869576b7e5")
                {
                    var q = db.Tbl_Student.Where(a => a.Student_Code == User.Identity.Name).SingleOrDefault();
                    q.Student_Form1 = true;
                    db.Entry(q).State = EntityState.Modified;
                }
                else if (_form.Form_Guid.ToString() == "de54c8ba-6032-4dc5-9241-2e6614a9840d")
                {
                    var q = db.Tbl_Student.Where(a => a.Student_Code == User.Identity.Name).SingleOrDefault();
                    q.Student_Form2 = true;
                    db.Entry(q).State = EntityState.Modified;
                }



                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شده";

                    if (_form.Form_Guid.ToString() == "c78afdf3-a647-4e54-95e8-94869576b7e5")
                    {
                        return RedirectToAction("ShowForm", new { ID = "de54c8ba-6032-4dc5-9241-2e6614a9840d" });
                    }
                    else if (_form.Form_Guid.ToString() == "de54c8ba-6032-4dc5-9241-2e6614a9840d")
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
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "خطا";

                    return RedirectToAction("ShowForm", new { ID = _form.Form_Guid });
                }
            }

            return View();
        }
    }
}
