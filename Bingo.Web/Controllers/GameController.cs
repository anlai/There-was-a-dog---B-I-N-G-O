using System;
using System.Linq;
using System.Web.Mvc;
using Bingo.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using Bingo.Web.Helpers;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the Game class
    /// </summary>
    //[Admin]
    public class GameController : ApplicationController
    {
	    private readonly IRepository<Game> _gameRepository;

        public GameController(IRepository<Game> gameRepository)
        {
            _gameRepository = gameRepository;
        }
    
        //
        // GET: /Game/
        public ActionResult Index()
        {
            var gameInProgress = CurrentGame();

            if (gameInProgress == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public ActionResult Current()
        {
            var gameInProgress = CurrentGame();

            var model = GameViewModel.Create(Repository);
            model.Game = gameInProgress;

            return View(model);
        }

        [HttpPost]
        public ActionResult PickNewNumber()
        {
            var gameInProgress = CurrentGame();

            var rand = new Random();
            var newNumber = rand.Next(1, 75);

            while (gameInProgress.CalledNumbersArray.Contains(newNumber))
            {
                //If the new number was already called, loop
                newNumber = rand.Next(1, 75);
            }

            gameInProgress.AddCalledNumber(newNumber);

            _gameRepository.EnsurePersistent(gameInProgress);

            Message = string.Format("Picked a new ball: {0}", newNumber.ToBingoBall());

            return RedirectToAction("Current");
        }

        private Game CurrentGame()
        {
            return _gameRepository.Queryable.Where(x => x.InProgress).SingleOrDefault();
        }
    }

    /// <summary>
    /// ViewModel for the Games class
    /// </summary>
    public class GameViewModel
    {
        public Game Game { get; set; }

        public static GameViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository must be supplied");

            var viewModel = new GameViewModel { Game = new Game() };

            return viewModel;
        }
    }
}
