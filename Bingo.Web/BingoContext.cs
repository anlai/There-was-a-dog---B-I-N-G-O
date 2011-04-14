using System.Data.Entity;
using Bingo.Web.Models;
using System;

namespace Bingo.Web
{
    public class BingoContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Bingo.Web.BingoContext>());

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameBall> GameBalls { get; set; }
        public DbSet<BingoCall> BingoCalls { get; set; }
    }

    public class BingoDbInitializer : DropCreateDatabaseAlways<BingoContext>
    {
        protected override void Seed(BingoContext context)
        {
            var scott = new User
                            {
                                Kerb = "postit",
                                Name = "Scott Kirkland",
                                Board = "8,10,5,3,1,23,25,20,18,16,38,40,0,33,31,53,55,50,48,46,68,70,65,63,61"
                            };
            var alan = new User
                           {
                               Kerb = "anlai",
                               Name = "Alan Lai",
                               Board = GameBoard.CreateSerializedString(GameBoard.Random())
                           };

            context.Users.Add(scott);
            context.Users.Add(alan);

            var game = new Game {InProgress = true, StartDate = DateTime.Now};
            game.GameBalls.Add(new GameBall {Game = game, Letter = "B", Number = 10, Picked = DateTime.Now});
            game.GameBalls.Add(new GameBall {Game = game, Letter = "N", Number = 38, Picked = DateTime.Now.AddSeconds(30)});
            game.GameBalls.Add(new GameBall {Game = game, Letter = "G", Number = 55, Picked = DateTime.Now.AddSeconds(60)});

            context.Games.Add(game);

            context.SaveChanges();
        }
    }
}
