using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wtiau.Health.Web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [Authorize(Roles ="Admin,SuperAdmin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}