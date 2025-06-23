using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trash_Track.Models;

namespace Trash_Track.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly TrashDBContext _context;

        public AdminController(TrashDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult Schedules()
        {
            var schedules = _context.PickupSchedules.Include(p => p.Ward).ToList();
            return View(schedules);
        }

        // POST: /Admin/UpdateSchedule
        [HttpPost]
        public IActionResult UpdateSchedule(int scheduleId, DayOfWeek day, TimeSpan time)
        {
            var schedule = _context.PickupSchedules.FirstOrDefault(p => p.Id == scheduleId);
            if (schedule != null)
            {
                schedule.PickupDay = day;
                schedule.PickupTime = time;
                _context.SaveChanges();
            }
            return RedirectToAction("Schedules");
        }
        public IActionResult Reports()
        {
            var reports = _context.Reports.Include(r => r.Ward).ToList();
            return View(reports);
        }
        [HttpPost]
        public IActionResult UpdateReportStatus(int reportId, string status)
        {
            var report = _context.Reports.FirstOrDefault(r => r.Id == reportId);
            if (report != null)
            {
                report.Status = status;
                _context.SaveChanges();
            }

            return RedirectToAction("Reports");
        }
        public IActionResult CreateOverride()
        {
            var wards = _context.Wards.OrderBy(w => w.No).ToList();

            var wardList = wards.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = $"Ward {w.No} - {w.Name}"
            }).ToList();

            ViewBag.Wards = wardList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOverride(PickupOverride model)
        {
            ModelState.Remove("Ward");

            if (ModelState.IsValid)
            {
                _context.PickupOverrides.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Overrides");
            }

            var wards = _context.Wards.OrderBy(w => w.No).ToList();
            var wardList = wards.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = $"Ward {w.No} - {w.Name}",
                Selected = w.Id == model.WardId
            }).ToList();

            ViewBag.Wards = wardList;

            return View(model);
        }
        public IActionResult Overrides()
        {
            var overrides = _context.PickupOverrides.Include(o => o.Ward).ToList();
            return View(overrides);
        }


    }
}
