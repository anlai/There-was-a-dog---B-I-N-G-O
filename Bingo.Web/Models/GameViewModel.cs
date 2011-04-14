using System;

namespace Bingo.Web.Models
{
    public class GameViewModel
    {
        public GameBoard GameBoard { get; set; }
        public DateTime ServerTime { get; set; }
        public string UserId { get; set; }

        public static GameViewModel Create(string userid, GameBoard gameBoard)
        {

            var viewModel = new GameViewModel()
                                {
                                    GameBoard = gameBoard ?? Models.GameBoard.Random(),
                                    ServerTime = DateTime.Now.AddSeconds(5),
                                    UserId = userid
                                };

            return viewModel;
        }
    }
}