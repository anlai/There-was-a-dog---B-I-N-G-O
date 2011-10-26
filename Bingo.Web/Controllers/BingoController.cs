using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Bingo.Web.Helpers;
using Bingo.Web.Models;
using System.Linq;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for all in game actions (polling functions)
    /// </summary>
    public class BingoController : ApplicationController
    {
        /// <summary>
        /// Function to get the next ball
        /// </summary>
        /// <remarks>Cache the result for 5 seconds</remarks>
        /// <returns>Returns the next ball and if the game is over</returns>
        [OutputCache(Duration = 5)]
        public JsonNetResult GetNextBall()
        {
            var currentBall = Db.GameBalls.Where(x => x.Game.InProgress).OrderByDescending(x=>x.Id).FirstOrDefault();

            var isCurrentGame = Db.Games.Where(x => x.InProgress).Any();

            // check in attendance
            if (isCurrentGame)
            {
                var currentGame = Db.Games.Where(a => a.InProgress).FirstOrDefault();

                var pastattendances = Db.Attandances.Where(a => a.User.Kerb == User.Identity.Name).ToList();
                foreach (var a in pastattendances) Db.Attandances.Remove(a);

                var user = Db.Users.Where(a => a.Kerb == User.Identity.Name).FirstOrDefault();
                var attendance = new Attendance() { User = user, Game = currentGame, InGame = true };
                
                Db.Attandances.Add(attendance);
                Db.SaveChanges();                
            }


            if (currentBall == null)
            {
                

                return isCurrentGame 
                    ? new JsonNetResult(new {ball = new GameBall {Number = -1}, gameover = false})
                    : new JsonNetResult(new { ball = new GameBall { Number = -1 }, gameover = true });
            }

            return new JsonNetResult(new { ball = currentBall, gameover = false });
        }
        
        /// <summary>
        /// A user believes and reports that they have bingo
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <param name="userId">User Id</param>
        /// <param name="numbers">The selected numbers by the player</param>
        /// <returns></returns>
        [HttpPost]
        public JsonNetResult ReportBingo(int id, string userId, List<int> numbers)
        {
            var game = Db.Games.Include("GameBalls").Where(x=>x.Id == id).Single();
            var user = Db.Users.Where(x=>x.Kerb == userId).Single();

            var validBingo = game.ValidateBingo(numbers.ToArray(), user);

            var call = new BingoCall {CalledAt = DateTime.Now, Callee = user, Game = game, ValidBoard = validBingo};

            Db.BingoCalls.Add(call);

            if (validBingo && game.InProgress)
            {
                game.Winner = user;
                game.InProgress = false;
                game.EndDate = DateTime.Now;

                Db.Entry(game).State = EntityState.Modified;
            }

            // add in a broadcast
            var msg = new Message() {Txt = string.Format("{0} has called bingo.", user.Name)};
            Db.Messages.Add(msg);

            if (!validBingo)
            {
                var msg2 = new Message() { Txt = string.Format("{0} does not have bingo.  Game will continue.", user.Name) };
                Db.Messages.Add(msg2);
            }

            Db.SaveChanges();

            return new JsonNetResult(validBingo);
        }

        /// <summary>
        /// Called when a page loads to get a user's client up to date with the current game
        /// </summary>
        /// <returns></returns>
        public JsonNetResult Initialize()
        {
            var game = Db.Games.Include("GameBalls").Where(a => a.InProgress).SingleOrDefault();

            if (game == null) return new JsonNetResult( new { nogame = true });

            var balls = game.GameBalls.Select(a=>new{a.Letter,a.Number}).ToList();

            var message = Db.Messages.OrderByDescending(a => a.Id).FirstOrDefault();
            var messageId = message == null ? 0 : message.Id;

            return new JsonNetResult(new {balls, gameId=game.Id, messageId});
        }

        /// <summary>
        /// Gets the latest message
        /// </summary>
        /// <param name="id">Id of the last message seen</param>
        /// <returns>One or more messages after the Id of the last one</returns>
        public JsonNetResult GetMessage(int id)
        {
            if (id == -1)
            {
                id = Db.Messages.Max(a => a.Id);
                return new JsonNetResult(new {id});
            }

            var messages = Db.Messages.Include("User").Where(a => a.Id > id).ToList();

            if (!messages.Any())
            {
                return new JsonNetResult(new {id});
            }

            var maxId = messages.Max(a => a.Id);

            return new JsonNetResult(new {id=maxId, messages});
        }

        /// <summary>
        /// Check to see if there is an active game, to update the waiting room button
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 5)]
        public JsonNetResult HasActiveGame()
        {
            var pastattendances = Db.Attandances.Where(a => a.User.Kerb == User.Identity.Name).ToList();
            foreach (var a in pastattendances) Db.Attandances.Remove(a);

            // check in attendance
            var user = Db.Users.Where(a => a.Kerb == User.Identity.Name).FirstOrDefault();
            var attendance = new Attendance() { User = user, Game = null, InGame = false };
            Db.Attandances.Add(attendance);
            Db.SaveChanges();

            return new JsonNetResult(Db.Games.Where(x=>x.InProgress).Any());
        }
    }
}