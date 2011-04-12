using FluentNHibernate.Mapping;

using UCDArch.Core.DomainModel;
using System.ComponentModel.DataAnnotations;

namespace Bingo.Web.Models
{
    public class User : DomainObject
    {
        public virtual string Kerb { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual string BingoBoard { get; set; }

        public virtual GameBoard GetBoard()
        {
            var board = new GameBoard {AllNumsAsString = BingoBoard};
            board.InitFromString();

            return board;
        }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);

            Map(x => x.Kerb);
            Map(x => x.Name);
            Map(x => x.BingoBoard).Column("Board");
        }
    }
}