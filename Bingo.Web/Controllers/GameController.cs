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

            if (gameInProgress == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
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

            while (gameInProgress.CalledNumbersArray.Contains(newNumber))
            {
                //If the new number was already called, loop
                newNumber = rand.Next(1, 75);
            }

            gameInProgress.AddCalledNumber(newNumber);
            Db.Entry(gameInProgress).State = EntityState.Modified;

            Db.SaveChanges();

            Message = string.Format("Picked a new ball: {0}", newNumber.ToBingoBall());

            return RedirectToAction("Current");
        }

        private Game CurrentGame()
        {
            return Db.Games.Where(x => x.InProgress).SingleOrDefault();
        }
    }
}
