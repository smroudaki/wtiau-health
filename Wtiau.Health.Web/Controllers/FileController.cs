using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wtiau.Health.Web.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        public FileResult Excel()
        {
            string path = Path.Combine(Server.MapPath("~/App_Data/Excel/"), "23.rar");

 

            return File(path, "*", string.Format("{0}.rar", "File"));
        }
    }
}