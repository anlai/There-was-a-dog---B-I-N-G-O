using System;
using System.Linq;
using System.Web.Mvc;
using Bingo.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the User class
    /// </summary>
    public class UserController : ApplicationController
    {
	    private readonly IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
    
        //
        // GET: /User/
        public ActionResult Index()
        {
            var userList = _userRepository.Queryable;

            return View(userList.ToList());
        }

        public ActionResult Create()
        {
            Message = CurrentUser.Identity.Name;
            var currentUserExists = _userRepository.Queryable.Any(x => x.Kerb == CurrentUser.Identity.Name);

            if (currentUserExists) throw new NotImplementedException();

            return View(new User());
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid) return View(user);

            var currentUserExists = _userRepository.Queryable.Any(x => x.Kerb == CurrentUser.Identity.Name);

            if (currentUserExists) throw new NotImplementedException();

            var newUser = new User
                                  {
                                      Kerb = CurrentUser.Identity.Name,
                                      Name = user.Name,
                                      BingoBoard = GameBoard.CreateSerializedString(GameBoard.Random())
                                  };

            _userRepository.EnsurePersistent(newUser);

            Message = "Your information was updated!";

            return RedirectToAction("Index", "Home");
        }
    }

	/// <summary>
    /// ViewModel for the User class
    /// </summary>
    public class UserViewModel
	{
		public User User { get; set; }
 
		public static UserViewModel Create(IRepository repository)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new UserViewModel {User = new User()};
 
			return viewModel;
		}
	}
}
