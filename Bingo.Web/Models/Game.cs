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
        public virtual bool InProgress { get; set; }
        public virtual DateTime? LastBallCalled { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        public ICollection<GameBall> GameBalls { get; set; }

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