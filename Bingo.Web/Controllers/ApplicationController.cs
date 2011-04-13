using System.Web.Mvc;

namespace Bingo.Web.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        public string Message
        {
            set { TempData["Message"] = value; }
        }
    }
}