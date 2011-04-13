using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bingo.Web.Helpers;
using Bingo.Web.Models;

namespace Bingo.Web.Controllers
{
    public class GameController : ApplicationController
    {
        //
        // GET: /Game/
        public ActionResult Index()
        {
            var gameInProgress = CurrentGame();

            return gameInProgress == null ? RedirectToAction("NewGame") : RedirectToAction("Current");
        }

        public ActionResult Current()
        {
            var gameInProgress = CurrentGame();

            return View(gameInProgress);
        }

        [HttpPost]
        public ActionResult PickNewNumber()
        {
            var gameInProgress = CurrentGame();

            var rand = new Random();
            var newNumber = rand.Next(1, 75);

            while (gameInProgress.GameBalls.Where(x=>x.Number == newNumber).Any())
            {
                //If the new number was already called, loop
                newNumber = rand.Next(1, 75);
            }
            
            var gameball = new GameBall {Game = gameInProgress, Letter = newNumber.ToBingoBallLetter(), Number = newNumber, Picked = DateTime.Now};
            
            gameInProgress.GameBalls.Add(gameball);
            Db.SaveChanges();

            Message = string.Format("Picked a new ball: {0}", newNumber.ToBingoBall());

            return RedirectToAction("Current");
        }

        private Game CurrentGame()
        {
            return Db.Games.Include("GameBalls").Where(x => x.InProgress).SingleOrDefault();
        }

        public ActionResult NewGame()
        {
            return View(new Game());
        }

        [HttpPost]
        public ActionResult CreateGame()
        {
            var game = new Game {InProgress = true, StartDate = DateTime.Now};

            Db.Games.Add(game);
            Db.SaveChanges();

            return RedirectToAction("Current");
        }
    }
}
