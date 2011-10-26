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
        public DbSet<Message> Messages { get; set; }
        public DbSet<Attendance> Attandances { get; set; }
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
                               Board = 	@"7,11,9,10,5
                                        ,22,26,24,25,20
                                        ,37,41,0,40,35
                                        ,52,56,54,55,50
                                        ,67,71,69,70,65"
                           };

            context.Users.Add(scott);
            context.Users.Add(alan);

            var game = new Game {InProgress = true, StartDate = DateTime.Now};

            // random balls
            game.GameBalls.Add(new GameBall { Game = game, Letter = "N", Number = 38, Picked = DateTime.Now.AddSeconds(30) });
            game.GameBalls.Add(new GameBall { Game = game, Letter = "G", Number = 55, Picked = DateTime.Now.AddSeconds(60) });

            // winning balls for scott
            game.GameBalls.Add(new GameBall { Game = game, Letter = "B", Number = 10, Picked = DateTime.Now});
            game.GameBalls.Add(new GameBall { Game = game, Letter = "B", Number = 8, Picked = DateTime.Now.AddSeconds(90) });
            game.GameBalls.Add(new GameBall { Game = game, Letter = "B", Number = 3, Picked = DateTime.Now.AddSeconds(120) });
            game.GameBalls.Add(new GameBall { Game = game, Letter = "B", Number = 5, Picked = DateTime.Now.AddSeconds(150) });
            game.GameBalls.Add(new GameBall { Game = game, Letter = "B", Number = 1, Picked = DateTime.Now.AddSeconds(180) });

            // wining balls for all
            game.GameBalls.Add(new GameBall { Game = game, Letter = "B", Number = 7, Picked = DateTime.Now.AddSeconds(180) });
            game.GameBalls.Add(new GameBall { Game = game, Letter = "I", Number = 26, Picked = DateTime.Now.AddSeconds(180) });
            game.GameBalls.Add(new GameBall { Game = game, Letter = "G", Number = 55, Picked = DateTime.Now.AddSeconds(180) });
            game.GameBalls.Add(new GameBall { Game = game, Letter = "O", Number = 65, Picked = DateTime.Now.AddSeconds(180) });

            context.Games.Add(game);

            var call1 = new BingoCall() {CalledAt = DateTime.Now, Callee = scott, Game = game, ValidBoard = false};
            var call2 = new BingoCall() { CalledAt = DateTime.Now.AddSeconds(10), Callee = alan, Game = game, ValidBoard = false };
            var call3 = new BingoCall() { CalledAt = DateTime.Now.AddSeconds(20), Callee = alan, Game = game, ValidBoard = true };

            context.BingoCalls.Add(call1);
            context.BingoCalls.Add(call2);
            context.BingoCalls.Add(call3);

            context.SaveChanges();
        }
    }
}
