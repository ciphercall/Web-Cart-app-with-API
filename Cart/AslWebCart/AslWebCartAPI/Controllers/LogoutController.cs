using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AslWebCartAPI.Controllers
{
    public class LogoutController : AppController
    {
        public ActionResult Index()
        {
            Session.Abandon();
            Global.GlobalVariable = 1;
            return RedirectToAction("Index", "Login");
        }
    }
}
