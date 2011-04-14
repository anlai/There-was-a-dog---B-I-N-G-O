using System;
using System.Collections.Generic;
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
        static readonly string [] Letters = new[] { "B", "I", "N", "G", "O" };
        static readonly List<Ball> AllBalls = new List<Ball>();

        //Cache the result for 5 seconds
        [OutputCache(Duration = 5)]
        public JsonNetResult GetNextBall()
        {
            var rand = new Random();
            var col = Letters[rand.Next(5)];

            var ball = new Ball { Letter = col, Number = rand.Next(80) };

            AllBalls.Add(ball);

            return new JsonNetResult(new { ball, gameover = false });
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
            return new JsonNetResult(true);
        }

        public JsonNetResult Initialize()
        {
            var game = Db.Games.Include("GameBalls").Where(a => a.InProgress).FirstOrDefault();
            var balls = game.GameBalls.Select(a=>new{Letter=a.Letter,Number=a.Number}).ToList();
            
            return new JsonNetResult(new {balls = balls, gameId=game.Id});
        }
    }
}