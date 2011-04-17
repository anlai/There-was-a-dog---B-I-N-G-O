using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bingo.Web.Models
{
    public class Message : DomainObject
    {
        public Message()
        {
            TimeofMessage = DateTime.Now;
        }

        public string Txt { get; set; }
        public User User { get; set; }
        public DateTime TimeofMessage { get; set; }
        /// <summary>
        /// Signifies if it is tied to a game
        /// </summary>
        public Game Game { get; set; }
    }
}