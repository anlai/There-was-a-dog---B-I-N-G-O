using System;
using System.Collections.Generic;
using System.Linq;

namespace Bingo.Web.Models
{
    /// <summary>
    /// The numbers in the:
    /// B column are between 1 and 15 
    /// I column between 16 and 30
    /// N column (containing four numbers and the free space) between 31 and 45
    /// G column between 46 and 60
    /// O column between 61 and 75.
    /// </summary>
    public class GameBoard
    {
        public int[] BCol { get; set; }
        public int[] ICol { get; set; }
        public int[] NCol { get; set; }
        public int[] GCol { get; set; }
        public int[] OCol { get; set; }

        public string AllNumsAsString { get; set; }

        public static GameBoard Random()
        {
            var gameboard = new GameBoard();

            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        gameboard.BCol = CreateNRandomNumbers(5, 1, 15);
                        break;
                    case 1:
                        gameboard.ICol = CreateNRandomNumbers(5, 16, 30);
                        break;
                    case 2:
                        gameboard.NCol = CreateNRandomNumbers(5, 31, 45);
                        break;
                    case 3:
                        gameboard.GCol = CreateNRandomNumbers(5, 46, 60);
                        break;
                    case 4:
                        gameboard.OCol = CreateNRandomNumbers(5, 61, 75);
                        break;
                }
            }

            //Add the free col
            gameboard.NCol[2] = 0;

            return gameboard;
        }

        private static int[] CreateNRandomNumbers(int n, int min, int max)
        {
            var rand = new Random();
            var array = new HashSet<int>();

            while (array.Count < n)
            {
                array.Add(rand.Next(min, max));
            }

            return array.ToArray();
        }

        public static string CreateSerializedString(GameBoard gameBoard)
        {
            var nums = new List<int>();
            nums.AddRange(gameBoard.BCol);
            nums.AddRange(gameBoard.ICol);
            nums.AddRange(gameBoard.NCol);
            nums.AddRange(gameBoard.GCol);
            nums.AddRange(gameBoard.OCol);

            return string.Join(",", nums);
        }

        public void InitFromString()
        {
            if (string.IsNullOrWhiteSpace(AllNumsAsString))
                throw new ArgumentNullException("AllNumsAsString must not be null");

            var nums = AllNumsAsString.Split(',').Select(s => int.Parse(s));

            BCol = nums.Skip(5*0).Take(5).ToArray();
            ICol = nums.Skip(5*1).Take(5).ToArray();
            NCol = nums.Skip(5*2).Take(5).ToArray();
            GCol = nums.Skip(5*3).Take(5).ToArray();
            OCol = nums.Skip(5*4).Take(5).ToArray();
        }
    }
}