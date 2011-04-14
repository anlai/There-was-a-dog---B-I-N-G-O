using System.Linq;
using System.Web.Mvc;
using Bingo.Web.Models;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the Client class
    /// </summary>
    public class PlayerController : ApplicationController
    {
        public ActionResult Game()
        {
            var user = Db.Users.Where(a => a.Kerb == User.Identity.Name).SingleOrDefault();

            if (user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View(GameViewModel.Create(User.Identity.Name, user.GetBoard()));
        }
    }
}
