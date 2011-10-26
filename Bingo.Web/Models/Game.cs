using System;
using System.Collections.Generic;
using System.Linq;

namespace Bingo.Web.Models
{
    public class Game : DomainObject
    {
        public Game()
        {
            GameBalls = new List<GameBall>();
        }

        //public virtual string CalledNumbers { get; set; }
        public bool InProgress { get; set; }
        public DateTime? LastBallCalled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public User Winner { get; set; }

        public ICollection<GameBall> GameBalls { get; set; }
        public ICollection<BingoCall> BingoCalls { get; set; }
        public ICollection<Attendance> Attendances { get; set; }

        public bool ValidateBingo(int[] chosenBalls, User user)
        {
            var userBoard = user.GetBoard();
            var board = userBoard.AsMatrix();

            for (int i = 0; i < 5; i++)
            {
                var numInARow = 0;
                var numInACol = 0;
                //For each row or call, see if there are 5 numbers on their gameboard that were also chosen
                for (int j = 0; j < 5; j++)
                {
                    if (j == 2 && i == 2) //Automatically give them the free square
                    {
                        numInARow++;
                        numInACol++;
                        continue;
                    }

                    var currentBallXAxis = board[i, j];
                    var currentBallYAxis = board[j, i];
                    
                    if (chosenBalls.Contains(currentBallXAxis))
                    {
                        //User ball matches, let's make sure it's a valid ball for this game
                        if (GameBalls.Where(x => x.Number == currentBallXAxis).Any())
                        {
                            //The ball matches one picked
                            numInARow++;
                        }
                    }

                    if (chosenBalls.Contains(currentBallYAxis))
                    {
                        //User ball matches, let's make sure it's a valid ball for this game
                        if (GameBalls.Where(x => x.Number == currentBallYAxis).Any())
                        {
                            //The ball matches one picked
                            numInACol++;
                        }
                    }
                }

                if (numInARow == 5 || numInACol == 5)
                {
                    //Bingo!!!
                    return true;
                }
            }

            var numInLr = 0;
            var numInRl = 0;

            for (int i = 0; i < 5; i++)
            {
                var currentBallLr = board[i, i];
                var currentBallRl = board[i, 4-i];

                if (i == 2) //Automatically give them the free square
                {
                    numInLr++;
                    numInRl++;
                    continue;
                }

                if (chosenBalls.Contains(currentBallLr))
                {
                    //User ball matches, let's make sure it's a valid ball for this game
                    if (GameBalls.Where(x => x.Number == currentBallLr).Any())
                    {
                        //The ball matches one picked
                        numInLr++;
                    }
                }

                if (chosenBalls.Contains(currentBallRl))
                {
                    //User ball matches, let's make sure it's a valid ball for this game
                    if (GameBalls.Where(x => x.Number == currentBallRl).Any())
                    {
                        //The ball matches one picked
                        numInRl++;
                    }
                }
            }

            if (numInLr == 5 || numInRl == 5)
            {
                return true;
            }

            return false;
        }

        /*
        public virtual HashSet<int> CalledNumbersArray
        {
            get
            {
                return string.IsNullOrWhiteSpace(CalledNumbers)
                           ? new HashSet<int>()
                           : new HashSet<int>(CalledNumbers.Split(',').Select(x => int.Parse(x)).ToList());
            }
        }

        public virtual void AddCalledNumber(int num)
        {
            if (string.IsNullOrWhiteSpace(CalledNumbers))
            {
                CalledNumbers = num.ToString();
            }
            else
            {
                var numbers = new List<string>(CalledNumbers.Split(',')) { num.ToString() };

                CalledNumbers = string.Join(",", numbers);
            }
        }
         */
    }
}