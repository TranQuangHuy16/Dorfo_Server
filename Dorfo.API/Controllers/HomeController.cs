using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
