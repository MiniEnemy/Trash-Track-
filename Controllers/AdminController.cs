using Microsoft.AspNetCore.Mvc;

namespace Trash_Track.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
