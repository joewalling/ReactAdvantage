using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReactAdvantage.Api.Models;

namespace ReactAdvantage.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(GraphqlPlayground));
        }

        public IActionResult GraphqlPlayground()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
