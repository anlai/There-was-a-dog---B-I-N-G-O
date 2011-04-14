using System.Linq;
using System.Web.Mvc;
using Bingo.Web.Models;
﻿using UCDArch.Core.PersistanceSupport;
﻿using UCDArch.Core.Utils;
﻿using UCDArch.Web.Attributes;

namespace Bingo.Web.Controllers
{
    /// <summary>
    /// Controller for the Client class
    /// </summary>
    public class PlayerController : ApplicationController
    {
        public ActionResult Game()
        {
            return View(GameViewModel.Create(User.Identity.Name));
        }
    }
}
