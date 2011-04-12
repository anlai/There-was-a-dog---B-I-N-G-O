using System.Linq;
using System.Web.Mvc;
using Bingo.Web.Models;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the Client class
    /// </summary>
    public class ClientController : ApplicationController
    {
        public ActionResult Game()
        {
            var board = GameBoard.Random();

            return View(board);
        }
    }
}
