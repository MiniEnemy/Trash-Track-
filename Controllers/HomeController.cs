using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trash_Track.Models;

namespace Trash_Track.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TrashDBContext _context;


        public HomeController(ILogger<HomeController> logger, TrashDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var wards = await _context.Wards
                .OrderBy(w => w.No)
                .Select(w => new SelectListItem
                {
                    Value = w.Id.ToString(),
                    Text = $"Ward {w.No} - {w.Name}"
                }).ToListAsync();

            ViewBag.Wards = wards;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public async Task<IActionResult> GetWardSchedule(int wardId)
        {
            try
            {
                var today = DateTime.Today;

                // Get the ward first
                var ward = await _context.Wards.FirstOrDefaultAsync(w => w.Id == wardId);
                if (ward == null)
                {
                    return Json(new { error = "Ward not found" });
                }

                // Get schedule using both WardId and Ward.No to match your data structure
                var schedule = await _context.PickupSchedules
                    .Include(p => p.Driver)
                    .Include(p => p.Ward)
                    .FirstOrDefaultAsync(p => p.WardId == wardId || p.Ward.No == ward.No);

                // Check for override today using both methods
                var overrideToday = await _context.PickupOverrides
                    .Include(o => o.Driver)
                    .Include(o => o.Ward)
                    .Where(o => (o.WardId == wardId || o.Ward.No == ward.No) &&
                               o.StartDate <= today && o.EndDate >= today)
                    .FirstOrDefaultAsync();

                // Handle TimeSpan formatting safely
                string overrideTimeStr = overrideToday?.NewTime?.ToString(@"hh\:mm") ??
                                       (schedule != null ? schedule.PickupTime.ToString(@"hh\:mm") : "Not scheduled");
                string regularTimeStr = schedule != null ? schedule.PickupTime.ToString(@"hh\:mm") : "Not scheduled";

                return Json(new
                {
                    isOverrideToday = overrideToday != null,
                    isCancelled = overrideToday?.IsCancelled ?? false,
                    overrideDay = overrideToday?.OverrideDay?.ToString() ?? schedule?.PickupDay.ToString(),
                    overrideTime = overrideTimeStr,
                    overrideDriver = overrideToday?.Driver?.Name ?? schedule?.Driver?.Name ?? "Not assigned",
                    overrideMessage = overrideToday?.Message ?? "",
                    regularDay = schedule?.PickupDay.ToString() ?? "Not scheduled",
                    regularTime = regularTimeStr,
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error getting ward schedule for wardId: {WardId}", wardId);
                return Json(new { error = "Unable to load schedule. Please try again." });
            }
        }
    }
}