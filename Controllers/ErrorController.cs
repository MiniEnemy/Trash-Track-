using Microsoft.AspNetCore.Mvc;

namespace Trash_Track.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
