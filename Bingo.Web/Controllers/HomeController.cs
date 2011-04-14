using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;

namespace Bingo.Web.Controllers
{
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            var info = "Info about this project: ";

            info += "<br/>Connection string: " + WebConfigurationManager.ConnectionStrings["BingoContext"].ConnectionString;
#if DEBUG
            info += "<br/>Debug Mode";
#endif
            ViewBag.Info = info;

            return View();
        }
    }
}
