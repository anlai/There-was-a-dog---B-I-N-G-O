﻿using System.Linq;
using System.Web.Mvc;
using Bingo.Web.Models;
using UCDArch.Web.Attributes;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the Client class
    /// </summary>
    public class PlayerController : ApplicationController
    {
        public ActionResult Game()
        {
            var board = GameBoard.Random();

            return View(board);
        }
    }
}
