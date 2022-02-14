using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtiau.Health.Web.Models.Domian;

namespace Wtiau.Health.Web.Models.Repository
{
    public static class Rep_UserRole
    {
        private static readonly HealthEntities db = new HealthEntities();
        public static string Get_RoleNameWithID(int id)
        {
            return db.Tbl_Role.Where(a => a.Role_ID == id).SingleOrDefault().Role_Name;
        }
    }
}
