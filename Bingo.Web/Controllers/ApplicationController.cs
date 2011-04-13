using System.Web.Mvc;

namespace Bingo.Web.Controllers
{
    [Authorize]
    public abstract class ApplicationController : Controller
    {
        protected readonly BingoContext Db = new BingoContext();

        public string Message
        {
            set { TempData["Message"] = value; }
        }

        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}