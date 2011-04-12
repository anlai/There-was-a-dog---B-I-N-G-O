using System.Linq;

namespace Bingo.Web.Models
{
    public class GameBoard
    {
        public int[] BCol { get; set; }
        public int[] ICol { get; set; }
        public int[] NCol { get; set; }
        public int[] GCol { get; set; }
        public int[] OCol { get; set; }

        public static GameBoard Random()
        {
            var gameboard = new GameBoard
                                {
                                    BCol = Enumerable.Range(1, 5).ToArray(),
                                    ICol = Enumerable.Range(21, 5).ToArray(),
                                    NCol = Enumerable.Range(41, 5).ToArray(),
                                    GCol = Enumerable.Range(61, 5).ToArray(),
                                    OCol = Enumerable.Range(81, 5).ToArray()
                                };

            return gameboard;
        }
    }
}