using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.LayUI.Controllers
{
    public class LayUiHomeController : Controller
    {
        // GET: LayUI/LayUiHome
        public ActionResult Index()
        {
            return View();
        }
    }
}