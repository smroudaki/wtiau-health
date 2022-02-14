using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Models.Repository
{
    public class Rep_User
    {
        private readonly HealthEntities  db = new HealthEntities();


        public Rep_User()
        {

        }

        public Model_AccountInfo GetInfoForNavbar(string Username)
        {
            var q = db.Tbl_Login.Where(a => a.Login_Email == Username || a.Login_Mobile == Username).SingleOrDefault();

            if (q != null)
            {
                Model_AccountInfo infoModel = new Model_AccountInfo();
                infoModel.Name = q.Login_Name + " " + q.Login_Family;
                infoModel.Role = q.Tbl_Role.Role_Display;
                return infoModel;
            }
            else
            {
                return null;
            }
        }

        public int Get_UserIDWithGUID(Guid guid)
        {
            return db.Tbl_Login.Where(x => x.Login_Guid == guid).SingleOrDefault().Login_ID;
        }

        public SelectListItem Get_UserSelectListItemWithGUID(Guid guid)
        {
            var q = db.Tbl_Login.Where(x => x.Login_Guid == guid).SingleOrDefault();
            return new SelectListItem() { Value = q.Login_Guid.ToString(), Text = q.Login_Name + " " + q.Login_Family};
        }

        public int Get_IDByUserName(string Username)
        {
            var id = db.Tbl_Login.Where(a => a.Login_Email == Username || a.Login_Mobile == Username).SingleOrDefault().Login_ID;

            return (int)id;
        }
    }
}
