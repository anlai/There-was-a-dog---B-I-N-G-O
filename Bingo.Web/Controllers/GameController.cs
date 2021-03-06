﻿using System;
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

            if (gameInProgress == null)
            {
                return RedirectToAction("GameEnded");
            }

            return View(gameInProgress);
        }

        public RedirectToRouteResult GameEnded()
        {
            Message =
                "There is no game in progress. One may have recently ended because of a successfull bingo call. See the Winner Board for details.";

            return RedirectToAction("NewGame");
        }

        [HttpPost]
        public ActionResult PickNewNumber()
        {
            var gameInProgress = CurrentGame();

            if (gameInProgress == null)
            {
                return RedirectToAction("GameEnded");
            }

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
            var waitingRoomViewModel = WaitingRoomViewModel.Create(Db);
            return View(waitingRoomViewModel);
        }

        [HttpPost]
        public ActionResult CreateGame()
        {
            var game = new Game {InProgress = true, StartDate = DateTime.Now};

            Db.Games.Add(game);
            Db.SaveChanges();

            return RedirectToAction("Current");
        }

        [HttpPost]
        public ActionResult CloseGame(int id)
        {
            var game = Db.Games.Find(id);

            if (game != null)
            {
                game.InProgress = false;
                Db.Entry(game).State = EntityState.Modified;
                Db.SaveChanges();

                Message = "Game closed!";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonNetResult CreateMessage(string message)
        {
            try
            {
                var user = Db.Users.Where(x => x.Kerb == User.Identity.Name).Single();
                var msg = new Message() { Txt = message, User = user };

                Db.Messages.Add(msg);
                Db.SaveChanges();

                return new JsonNetResult(true);
            }
            catch { }

            return new JsonNetResult(false);
        }

        public JsonNetResult GetGameAtteandance()
        {
            var attendances = from a in Db.Attandances
                              where a.InGame 
                              group a by a.User into x
                              select new { Name = x.Key.Name, Update = x.Max(y => y.LastUpdate) };

            // only want people that have checked in in the last 30 seconds.
            var result = attendances.OrderBy(a => a.Name).ToList();
            result = result.Where(a => a.Update.AddSeconds(30d) > DateTime.Now).ToList();

            return new JsonNetResult(result.Select(a => new { Name = a.Name, Update = a.Update.ToString("g") }));
        }

        public JsonNetResult GetWaitingAtteandance()
        {
            var attendances = from a in Db.Attandances
                              where !a.InGame
                              group a by a.User into x
                              select new { Name = x.Key.Name, Update = x.Max(y => y.LastUpdate) };

            // only want people that have checked in in the last 30 seconds.
            var result = attendances.OrderBy(a => a.Name).ToList();
            result = result.Where(a => a.Update.AddSeconds(30d) > DateTime.Now).ToList();

            return new JsonNetResult(result.Select(a => new { Name = a.Name, Update = a.Update.ToString("g") }));
        }

    }
}
