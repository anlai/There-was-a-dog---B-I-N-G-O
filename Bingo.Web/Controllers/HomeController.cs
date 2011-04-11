using System.Web.Mvc;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the Home class
    /// </summary>
    public class HomeController : ApplicationController
    {
        public ViewResult Sample()
        {
            return View();
        }
    }
}
