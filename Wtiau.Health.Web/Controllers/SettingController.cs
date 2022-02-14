using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SettingController : Controller
    {
        readonly HealthEntities db = new HealthEntities();

        public ActionResult Index()
        {
            return View();
        }

        #region Student



        #endregion

        #region Role

        public ActionResult Role_List()
        {
            var model = db.Tbl_Role.Where(x => x.Role_IsDelete == false).Select(x => new Model_RoleList
            {
                ID = x.Role_ID,
                Display = x.Role_Display,
                Name = x.Role_Name,
                Level = x.Role_Level

            }).ToList();

            return View(model);
        }

        #endregion

        #region User

        public ActionResult User_List()
        {
            var model = db.Tbl_Login.Where(x => x.Login_IsDelete == false).Select(x => new Model_UserList
            {
                ID = x.Login_ID,
                Family = x.Login_Family,
                Name = x.Login_Name,
                Role = x.Tbl_Role.Role_Display,
                Email = x.Login_Email,
                Mobile = x.Login_Mobile

            }).ToList();

            return View(model);
        }

        public ActionResult User_Add()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult User_Add(Model_UserAdd model)
        {
            if (ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tbl_Login _Login = new Tbl_Login
            {
                Login_Guid = Guid.NewGuid(),
                Login_Email = model.Email,
                Login_Name = model.Name,
                Login_Family = model.Family,
                Login_Mobile = model.Mobile,
                Login_RoleID = model.Role,
                Login_CreateDate = DateTime.Now,
                Login_Modify = DateTime.Now
            };

            var Salt = Guid.NewGuid().ToString("N");
            var SaltPassword = model.Password + Salt;
            var SaltPasswordBytes = Encoding.UTF8.GetBytes(SaltPassword);
            var SaltPasswordHush = Convert.ToBase64String(SHA512.Create().ComputeHash(SaltPasswordBytes));

            _Login.Login_PasswordHash = SaltPasswordHush;
            _Login.Login_PasswordSalt = Salt;

            db.Tbl_Login.Add(_Login);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "کاربر جدید با موفقیت اضافه شد.";

                return RedirectToAction("User_List");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "کاربر جدید با موفقیت اضافه نشد.";

                return View();
            }
        }

        public ActionResult User_Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (db.Tbl_Login.Any(a => a.Login_ID == id))
            {
                var model = db.Tbl_Login.Where(a => a.Login_ID == id).Select(x => new Model_UserEdit
                {
                    ID = x.Login_ID,
                    Family = x.Login_Family,
                    Name = x.Login_Name,
                    Role = x.Login_RoleID,
                    Email = x.Login_Email,
                    Mobile = x.Login_Mobile
                    
                }).SingleOrDefault();

                return PartialView(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult User_Edit(Model_UserEdit model)
        {
            if (ModelState.IsValid)
            {
                var _Login = db.Tbl_Login.Where(x => x.Login_ID == model.ID && !x.Login_IsDelete).SingleOrDefault();

                if (_Login != null)
                {
                    _Login.Login_Name = model.Name;
                    _Login.Login_Family = model.Family;
                    _Login.Login_Mobile = model.Mobile;
                    _Login.Login_Email = model.Email;
                    _Login.Login_RoleID = model.Role;
                    _Login.Login_Modify = DateTime.Now;

                    db.Entry(_Login).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کاربر مورد نظر با موفقیت ویرایش شد.";
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کاربر مورد نظر با موفقیت ویرایش نشد.";
                    }

                    return RedirectToAction("User_List");
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult User_Delete(int? id)
        {
            if (id.HasValue)
            {
                Model_MessageModal model = new Model_MessageModal();

                var _Login = db.Tbl_Login.Where(x => x.Login_ID == id).SingleOrDefault();

                if (_Login != null)
                {
                    model.ID = id.Value;
                    model.Name = _Login.Login_Name + " " + _Login.Login_Family;
                    model.Description = "آیا از حذف کاربر مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult User_Delete(Model_MessageModal model)
        {
            if (ModelState.IsValid)
            {
                var _Login = db.Tbl_Login.Where(x => x.Login_ID == model.ID && !x.Login_IsDelete).SingleOrDefault();

                if (_Login != null)
                {
                    _Login.Login_IsDelete = true;

                    db.Entry(_Login).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کاربر مورد نظر با موفقیت حذف شد.";
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کاربر مورد نظر با موفقیت حذف نشد.";
                    }

                    return RedirectToAction("User_List");
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        #endregion

    }
}