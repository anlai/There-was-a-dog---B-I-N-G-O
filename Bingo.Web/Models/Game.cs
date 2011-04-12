using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Bingo.Web.Models
{
    public class Game : DomainObject
    {
        public virtual string CalledNumbers { get; set; }
        public virtual bool InProgress { get; set; }
        public virtual DateTime? LastBallCalled { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

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
                var numbers = new List<string>(CalledNumbers.Split(',')) {num.ToString()};

                CalledNumbers = string.Join(",", numbers);
            }
        }
    }

    public class GameMap : ClassMap<Game>
    {
        public GameMap()
        {
            Id(x => x.Id);

            Map(x => x.CalledNumbers);
            Map(x => x.InProgress);
            Map(x => x.LastBallCalled);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
        }
    }
}