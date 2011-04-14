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
    /// Controller for the Bingo class
    /// </summary>
    public class BingoController : ApplicationController
    {
        //Cache the result for 5 seconds
        [OutputCache(Duration = 5)]
        public JsonNetResult GetNextBall()
        {
            var currentBall = Db.GameBalls.Where(x => x.Game.InProgress).OrderByDescending(x=>x.Id).FirstOrDefault();

            if (currentBall == null)
            {
                var isCurrentGame = Db.Games.Where(x => x.InProgress).Any();

                return isCurrentGame 
                    ? new JsonNetResult(new {ball = new GameBall {Number = -1}, gameover = false})
                    : new JsonNetResult(new { ball = new GameBall { Number = -1 }, gameover = true });
            }

            return new JsonNetResult(new { ball = currentBall, gameover = false });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Game Id</param>
        /// <param name="userId">User Id</param>
        /// <param name="numbers">The selected numbers by the player</param>
        /// <returns></returns>
        [HttpPost]
        public JsonNetResult ReportBingo(int id, string userId, List<int> numbers)
        {
            var game = Db.Games.Where(a => a.InProgress).Single();
            var user = Db.Users.Find(userId);

            var validBingo = game.ValidateBingo(numbers.ToArray(), user);

            var call = new BingoCall {CalledAt = DateTime.Now, Callee = user, Game = game, ValidBoard = validBingo};

            Db.BingoCalls.Add(call);

            if (validBingo)
            {
                game.Winner = user;
                game.InProgress = false;
                game.EndDate = DateTime.Now;

                Db.Entry(game).State = EntityState.Modified;
            }

            Db.SaveChanges();

            return new JsonNetResult(validBingo);
        }

        public JsonNetResult Initialize()
        {
            var game = Db.Games.Include("GameBalls").Where(a => a.InProgress).SingleOrDefault();
            var balls = game.GameBalls.Select(a=>new{a.Letter,a.Number}).ToList();
            
            return new JsonNetResult(new {balls, gameId=game.Id});
        }
    }
}