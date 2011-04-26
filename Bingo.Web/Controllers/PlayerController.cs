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
        /// <summary>
        /// The actual game board
        /// </summary>
        /// <returns></returns>
        public ActionResult Game()
        {
            var user = Db.Users.Where(a => a.Kerb == User.Identity.Name).SingleOrDefault();

            if (user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var message = new Message() {Txt = string.Format("{0} has just joined the game.", user.Name)};

            return View(GameViewModel.Create(User.Identity.Name, user.GetBoard()));
        }

        /// <summary>
        /// Waiting room
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(WaitingRoomViewModel.Create(Db));
        }
    }
}
