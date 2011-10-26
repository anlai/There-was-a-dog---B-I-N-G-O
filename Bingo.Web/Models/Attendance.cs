using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bingo.Web.Models
{
    public class Attendance : DomainObject
    {
        public Attendance()
        {
            LastUpdate = DateTime.Now;
        }

        public User User { get; set; }
        public DateTime LastUpdate { get; set; }
        public Game Game { get; set; }
        public bool InGame { get; set; }
    }
}