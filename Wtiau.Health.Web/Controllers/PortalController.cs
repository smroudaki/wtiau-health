using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wtiau.Health.Web.Controllers
{
    public class PortalController : Controller
    {
        // GET: Portal
        public ActionResult Index()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PortalLogin", "Account");
            }
            return View();
        }
    }
}