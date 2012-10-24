using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bingo.Web.Models;

namespace Bingo.Web.Controllers
{
    public class GameBoardController : ApplicationController
    {
        //
        // GET: /GameBoard/

        public ActionResult Index()
        {
            return View(GameBoard.Random());
        }

    }
}
