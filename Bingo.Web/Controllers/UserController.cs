using System;
using System.Linq;
using System.Web.Mvc;
using Bingo.Web.Models;

namespace Bingo.Web.Controllers
{ 
    public class UserController : ApplicationController
    {
        private readonly BingoContext _db = new BingoContext();

        //
        // GET: /User/
        public ViewResult Index()
        {
            return View(_db.Users.ToList());
        }

        public ActionResult Create()
        {
            if (CurrentUserExists()) throw new NotImplementedException();

            return View(new User { Kerb = ControllerContext.HttpContext.User.Identity.Name });
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid) return View(user);

            if (CurrentUserExists()) throw new NotImplementedException();

            var newUser = new User
            {
                Kerb = ControllerContext.HttpContext.User.Identity.Name,
                Name = user.Name,
                Board = GameBoard.CreateSerializedString(GameBoard.Random())
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            Message = "Your information was updated!";

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details(int id)
        {
            return View(_db.Users.Find(id));
        }

        private bool CurrentUserExists()
        {
            var currentUserName = ControllerContext.HttpContext.User.Identity.Name;
            
            return _db.Users.Any(x => x.Kerb == currentUserName);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}