using System;

namespace Bingo.Web.Models
{
    public class GameViewModel
    {
        public GameBoard GameBoard { get; set; }
        public DateTime ServerTime { get; set; }
        public string UserId { get; set; }

        public static GameViewModel Create(string userid)
        {
            var viewModel = new GameViewModel()
                                {
                                    GameBoard = Models.GameBoard.Random(),
                                    ServerTime = DateTime.Now,
                                    UserId = userid
                                };

            return viewModel;
        }
    }
}