using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UCDArch.Web.ActionResults;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the Bingo class
    /// </summary>
    public class BingoController : ApplicationController
    {
        static readonly char[] Letters = new[] { 'B', 'I', 'N', 'G', 'O' };
        static readonly List<Ball> AllBalls = new List<Ball>();

        //Cache the result for 5 seconds
        [OutputCache(Duration = 5)]
        public JsonNetResult GetNextBall()
        {
            var rand = new Random();
            var col = Letters[rand.Next(5)];

            var ball = new Ball { Letter = col, Number = rand.Next(80) };

            AllBalls.Add(ball);

            return new JsonNetResult(new {ball, gameover = false});
        }

        public JsonNetResult Initialize()
        {
            return new JsonNetResult(AllBalls);
        }
    }

    public class Ball
    {
        public char Letter { get; set; }

        public int Number { get; set; }

        public override bool Equals(object obj)
        {
            var that = obj as Ball;

            if (that == null)
                return false;

            return Letter == that.Letter && Number == that.Number;
        }

        public override int GetHashCode()
        {
            return Letter.GetHashCode() + 27*Number.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Letter, Number);
        }
    }
}
