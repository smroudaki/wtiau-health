using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Models.Repository
{
    public class Rep_Dashboard
    {
        private readonly HealthEntities db = new HealthEntities();

        public Rep_Dashboard()
        {

        }

        public Model_DashBoardInfo info()
        {
            Model_DashBoardInfo model = new Model_DashBoardInfo();
            model.StudentInSystemCount = db.Tbl_Student.Count();
            model.StudentInfoCount = db.Tbl_StudentInfo.Count();
            model.TakeTimeCount = db.Tbl_TakeTurn.Count();

            return model;

        }

    }
}