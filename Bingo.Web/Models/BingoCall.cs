using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bingo.Web.Models
{
    public class BingoCall : DomainObject
    {
        public User Callee { get; set; }
        public Game Game { get; set; }
        public DateTime CalledAt { get; set; }
        public bool ValidBoard { get; set; }
    }
}
