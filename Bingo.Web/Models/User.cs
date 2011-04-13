using System.ComponentModel.DataAnnotations;

namespace Bingo.Web.Models
{
    public class User : DomainObject
    {
        [Required]
        public string Kerb { get; set; }
        public string Name { get; set; }
        public string Board { get; set; }

        public GameBoard GetBoard()
        {
            var board = new GameBoard { AllNumsAsString = Board };
            board.InitFromString();

            return board;
        }
    }
}