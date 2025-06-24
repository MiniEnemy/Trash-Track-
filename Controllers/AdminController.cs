using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trash_Track.Models;
using Trash_Track.Utility;

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

        public async Task<IActionResult> AdminReports()
        {
            var reports = await _context.Reports
                .Include(r => r.Ward)
                .Include(r => r.AssignedDriver)
                .ToListAsync();

            ViewBag.StatusList = Trash_Track.Utility.ReportStatuses.All;
            ViewBag.Drivers = _context.Drivers
    .Select(d => new SelectListItem
    {
        Value = d.Id.ToString(),
        Text = d.Name
    }).ToList();

            return View(reports);
        }

        [HttpPost]
        public IActionResult UpdateReport(int reportId, string status, int assignedDriverId, string? remarks)
        {
            if (!ReportStatuses.All.Contains(status))
                return BadRequest("Invalid status.");

            var report = _context.Reports.FirstOrDefault(r => r.Id == reportId);
            if (report != null)
            {
                report.Status = status;
                report.AssignedDriverId = assignedDriverId;
                report.Remarks = remarks;
                _context.SaveChanges();
            }

            return RedirectToAction("AdminReports");
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

            // ✅ Check for duplicate override
            bool duplicate = _context.PickupOverrides.Any(o =>
                o.WardId == model.WardId &&
                model.StartDate <= o.EndDate &&
                model.EndDate >= o.StartDate
            );

            if (duplicate)
            {
                ModelState.AddModelError("", "An override already exists for this ward in the selected date range.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Wards = _context.Wards
                    .OrderBy(w => w.No)
                    .Select(w => new SelectListItem
                    {
                        Value = w.Id.ToString(),
                        Text = $"Ward {w.No} - {w.Name}",
                        Selected = w.Id == model.WardId
                    })
                    .ToList();

                ViewBag.Drivers = new SelectList(_context.Drivers, "Id", "Name");
                ViewBag.DayList = Enum.GetValues(typeof(DayOfWeek))
    .Cast<DayOfWeek>()
    .Select(d => new SelectListItem
    {
        Value = d.ToString(),
        Text = d.ToString()
    }).ToList();


                return View(model);
            }

            _context.PickupOverrides.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Overrides");
        }

        public IActionResult Overrides()
        {
            var overrides = _context.PickupOverrides
                .Include(o => o.Ward)
                    .ThenInclude(w => w.PickupSchedule)
                        .ThenInclude(s => s.Driver)
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

            bool overlapping = _context.PickupOverrides.Any(o =>
                o.Id != model.Id && // ✅ Exclude current override
                o.WardId == model.WardId &&
                model.StartDate <= o.EndDate &&
                model.EndDate >= o.StartDate
            );

            if (overlapping)
            {
                ModelState.AddModelError("", "Another override exists for this ward in this date range.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Wards = new SelectList(_context.Wards.OrderBy(w => w.No), "Id", "No", model.WardId);
                ViewBag.Drivers = new SelectList(_context.Drivers, "Id", "Name");
                ViewBag.DayList = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(d =>
                    new SelectListItem { Text = d.ToString(), Value = ((int)d).ToString() }).ToList();

                return View(model);
            }

            _context.PickupOverrides.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Overrides");
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
