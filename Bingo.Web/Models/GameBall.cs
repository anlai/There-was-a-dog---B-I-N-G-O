using System;

namespace Bingo.Web.Models
{
    public class GameBall : Ball
    {
        public int Id { get; set; }
        public DateTime Picked { get; set; }
        public Game Game { get; set; }
    }
}