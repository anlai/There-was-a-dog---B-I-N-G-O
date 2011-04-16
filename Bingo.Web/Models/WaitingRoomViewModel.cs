using System.Collections.Generic;
using System.Linq;

namespace Bingo.Web.Models
{
    public class WaitingRoomViewModel
    {
        // current game
        public Game Game { get; set; }
        // winner board, past games and their winners
        public IList<BingoCall> BingoCalls { get; set; }

        public static WaitingRoomViewModel Create(BingoContext db)
        {

            var calls = (from call in db.BingoCalls
                         where db.BingoCalls.Where(x => x.ValidBoard).Select(x => x.Game.Id).Contains(call.Game.Id)
                         group call by call.Game.Id
                             into game
                             select
                                 db.BingoCalls.Where(x => x.Game.Id == game.Key && x.ValidBoard && x.CalledAt == game.Where(y => y.ValidBoard).Min(y => y.CalledAt)).FirstOrDefault()
                        ).Select(x=>x.Id).ToList();

            var viewModel = new WaitingRoomViewModel()
                                {
                                    Game = db.Games.Where(a => a.InProgress).SingleOrDefault(),
                                    BingoCalls = db.BingoCalls.Include("Game").Include("Callee").Where(a=> calls.Contains(a.Id)).ToList()
                                };

            return viewModel;
        }
    }
}