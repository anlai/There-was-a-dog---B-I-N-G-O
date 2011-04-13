using System.ComponentModel.DataAnnotations;

namespace Bingo.Web.Models
{
    public class User : DomainObject
    {
        [Required]
        public string Kerb { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Board { get; set; }

        public GameBoard GetBoard()
        {
            var board = new GameBoard { AllNumsAsString = Board };
            board.InitFromString();

            return board;
        }
    }
}