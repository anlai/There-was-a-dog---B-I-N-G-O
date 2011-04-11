using System;
using System.Linq;
using System.Web.Mvc;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the Client class
    /// </summary>
    public class ClientController : ApplicationController
    {
        public ActionResult Game()
        {
            var viewModel = ClientViewModel.Create(Repository);
            return View(viewModel);
        }
    }

    public class ClientViewModel
    {
        public static ClientViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new ClientViewModel();

            return viewModel;
        }
    }
}
