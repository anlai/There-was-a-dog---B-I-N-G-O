using System.Linq;
using Bingo.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bingo.Tests
{
    [TestClass]
    public class ValidateBingoTests
    {
        [TestMethod]
        public void WillRejectEmptyGame()
        {
            var user = GetUser();
            var game = new Game();

            var userChosenBalls = Enumerable.Range(1, 75).ToArray();

            Assert.IsFalse(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void GameAndUserWithEverythingWillValidate()
        {
            var user = GetUser();
            var game = GetGameWithAllNumbersPicked();

            var userChosenBalls = Enumerable.Range(1, 75).ToArray();

            Assert.IsTrue(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void OnlyFiveInARowOnAFullBoardIsBingo()
        {
            var user = GetUser();
            var game = GetGameWithAllNumbersPicked();

            var userChosenBalls = new int[5] {8, 23, 38, 53, 68};
            
            Assert.IsTrue(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void OnlyFiveInAColOnAFullBoardIsBingo()
        {
            var user = GetUser();
            var game = GetGameWithAllNumbersPicked();

            var userChosenBalls = new int[5] {8, 10, 11, 1, 7};

            Assert.IsTrue(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void OnlyFourInAColOnAFullBoardIsNotBingo()
        {
            var user = GetUser();
            var game = GetGameWithAllNumbersPicked();

            var userChosenBalls = new int[4] { 8, 10, 1, 7 };

            Assert.IsFalse(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void OnlyFiveInAColOnAFullBoardWithOneIncorrectBallIsNotBingo()
        {
            var user = GetUser();
            var game = GetGameWithAllNumbersPicked();

            var userChosenBalls = new int[5] { 8, 10, 11, 2 /*user does not have 2*/, 7 };

            Assert.IsFalse(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void OnlyFiveInAColOnANearlyFullBoardExceptOneNecessaryBallIsNotBingo()
        {
            var user = GetUser();
            var game = GetGameWithAllNumbersPicked();

            var userChosenBalls = new int[5] { 8, 10, 11, 1, 7 }; //valid balls

            game.GameBalls.Remove(game.GameBalls.Where(x => x.Number == 7).Single()); //remove 7, so that's not a valid choice

            Assert.IsFalse(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void OnlyFourInAColOnAFullBoardWithFreeSquareIsBingo()
        {
            var user = GetUser();
            var game = GetGameWithAllNumbersPicked();

            var userChosenBalls = new int[4] { 38, 40, 31, 37 };

            Assert.IsTrue(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void OnlyFourInARowOnAFullBoardWithFreeSquareIsBingo()
        {
            var user = GetUser();
            var game = GetGameWithAllNumbersPicked();

            var userChosenBalls = new int[4] { 11, 26, 56, 71 };

            Assert.IsTrue(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void OnlyFourInADiagonalOnAFullBoardIsBingo()
        {
            var user = GetUser();
            var game = GetGameWithAllNumbersPicked();

            var userChosenBalls = new int[4] { 8, 25, 46, 67 };

            Assert.IsTrue(game.ValidateBingo(userChosenBalls, user));
        }

        [TestMethod]
        public void ValidatesMyBoard()
        {
            var user = new User {Board = "8,10,5,3,1,23,25,20,18,16,38,40,0,33,31,53,55,50,48,46,68,70,65,63,61", Kerb = "postit"};
            var game = new Game();
            game.GameBalls = new GameBall[]
                                 {
                                     new GameBall {Number = 10}, new GameBall {Number = 25}, new GameBall {Number = 40},
                                     new GameBall {Number = 55}, new GameBall {Number = 70}
                                 };

            var userChosenBalls = new int[] { 38, 10, 25, 40, 55, 70, 18 };

            Assert.IsTrue(game.ValidateBingo(userChosenBalls, user));
        }

        private static User GetUser()
        {
            return new User { Board = "8,10,11,1,7,23,25,26,16,22,38,40,0,31,37,53,55,56,46,52,68,70,71,61,67" };
        }

        private static Game GetGameWithAllNumbersPicked()
        {
            var game = new Game();

            for (int i = 1; i <= 75; i++)
            {
                game.GameBalls.Add(new GameBall {Game = game, Number = i});
            }

            return game;
        }
    }
}
