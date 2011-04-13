using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Bingo.Web.Models;
using UCDArch.Web.ActionResults;

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

        public JsonNetResult Initialize()
        {
            return new JsonNetResult(AllBalls);
        }
    }
}