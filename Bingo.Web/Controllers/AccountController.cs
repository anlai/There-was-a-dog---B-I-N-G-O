using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.Authentication;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the Account class
    /// </summary>
    public class AccountController : Controller
    {
        public string Message
        {
            set { TempData["Message"] = value; }
        }

        public ActionResult LogOn()
        {
            string resultUrl = CASHelper.Login(); //Do the CAS Login

            if (resultUrl != null)
            {
                return Redirect(resultUrl);
            }

            TempData["URL"] = resultUrl;

            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("https://cas.ucdavis.edu/cas/logout");
        }

        public RedirectToRouteResult Emulate(string id /* Login ID*/)
        {
            if (User.IsInRole("EmulationUser"))
            {
                if (!string.IsNullOrEmpty(id))
                {
                    Message = "Emulating " + id;
                    FormsAuthentication.RedirectFromLoginPage(id, false);
                }
                else
                {
                    Message = "Login ID not provided.  Use /Emulate/login";
                }
            }
            else
            {
                Message = "You do not have permission to perform this action";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
