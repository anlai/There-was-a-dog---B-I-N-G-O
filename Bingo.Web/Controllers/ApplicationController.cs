using UCDArch.Web.Controller;
using System.Web.Mvc;
using UCDArch.Web.Attributes;

namespace Bingo.Web.Controllers
{
    [Authorize]
    [HandleTransactionsManually]
    public class ApplicationController : SuperController { }
}