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
            ViewBag.wardCount = _context.Wards.Count();
            ViewBag.reportCount = _context.Reports.Count();
            ViewBag.userCount = _context.Users.Count();

            return View();
        } 
        public IActionResult Schedules()
        {
            var schedules = _context.PickupSchedules.Include(p => p.Ward).ToList();
            ViewBag.Drivers = new SelectList(_context.Drivers.ToList(), "Id", "Name");

            return View(schedules);
        }

        // POST: /Admin/UpdateSchedule
        [HttpPost]
        public IActionResult UpdateSchedule(int scheduleId, DayOfWeek day, TimeSpan time, int driverId)
        {
            var schedule = _context.PickupSchedules.Find(scheduleId);
            if (schedule != null)
            {
                schedule.PickupDay = day;
                schedule.PickupTime = time;
                schedule.DriverId = driverId;
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
            ViewBag.Drivers = new SelectList(_context.Drivers, "Id", "Name");


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
            var duplicate = _context.PickupOverrides.Any(o =>
    o.WardId == model.WardId &&
    model.StartDate <= o.EndDate && model.EndDate >= o.StartDate);

            if (duplicate)
            {
                ModelState.AddModelError("", "An override already exists for this ward in that time range.");
            }


            var wards = _context.Wards.OrderBy(w => w.No).ToList();
            var wardList = wards.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = $"Ward {w.No} - {w.Name}",
                Selected = w.Id == model.WardId
            }).ToList();

            ViewBag.Wards = wardList;
            ViewBag.Drivers = new SelectList(_context.Drivers, "Id", "Name");


            return View(model);
        }
        public IActionResult Overrides()
        {
            var overrides = _context.PickupOverrides
                .Include(o => o.Ward)
                .Include(o => o.Driver) 
                .ToList();

            ViewBag.Drivers = new SelectList(_context.Drivers, "Id", "Name");
            return View(overrides);
        }


        public IActionResult EditOverride(int id)
        {
            var overrideItem = _context.PickupOverrides.Find(id);
            if (overrideItem == null) return NotFound();

            ViewBag.Wards = new SelectList(_context.Wards.OrderBy(w => w.No), "Id", "No", overrideItem.WardId);
            ViewBag.Drivers = new SelectList(_context.Drivers, "Id", "Name");

            return View(overrideItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOverride(PickupOverride model)
        {
            ModelState.Remove("Ward");

            if (ModelState.IsValid)
            {
                _context.PickupOverrides.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Overrides");
            }

            ViewBag.Wards = new SelectList(_context.Wards.OrderBy(w => w.No), "Id", "No", model.WardId);
            ViewBag.Drivers = new SelectList(_context.Drivers, "Id", "Name");

            return View(model);
        }
        public IActionResult DeleteOverride(int id)
        {
            var item = _context.PickupOverrides
                .Include(p => p.Ward)
                .FirstOrDefault(p => p.Id == id);

            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOverride(PickupOverride model)
        {
            var item = _context.PickupOverrides.Find(model.Id);
            if (item != null)
            {
                _context.PickupOverrides.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Overrides");
        }


    }
}
