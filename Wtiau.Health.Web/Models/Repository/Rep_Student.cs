using System.Linq;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Models.Repository
{
    public class Rep_Student
    {
        private readonly HealthEntities db = new HealthEntities();

        public Rep_Student()
        {

        }

        public Model_AccountInfo GetInfoForNavbar(string Username)
        {
            var q = db.Tbl_Student.Where(a => a.Student_Code == Username).SingleOrDefault();

            if (q != null)
            {
                Model_AccountInfo infoModel = new Model_AccountInfo();

                infoModel.Name = q.Student_Code;

                if (q.Student_SIID != null)
                {
                    infoModel.Name = q.Tbl_StudentInfo.SI_Name + " " + q.Tbl_StudentInfo.SI_Family;
                }

                infoModel.Role = "دانشجو";
                return infoModel;
            }
            else
            {
                return null;

            }

        }
    }
}