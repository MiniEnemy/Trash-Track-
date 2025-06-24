using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trash_Track.Models;
using Trash_Track.Models.Trash_Track.Models;

namespace Trash_Track.Controllers
{
    public class DriverController : Controller
    {
        private readonly TrashDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DriverController(TrashDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
    
        
        
        public async Task<IActionResult> PickupChecklist()
        {
            var userId = _userManager.GetUserId(User);
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
                return NotFound("Driver not found.");

            var today = DateTime.Today.DayOfWeek;
            var count = await _context.PickupSchedules
                .Where(p => p.DriverId == driver.Id && p.PickupDay == today)
                .CountAsync();

            Console.WriteLine($"Found {count} schedules for today.");


            
            var schedules = await _context.PickupSchedules
                .Include(p => p.Ward)
                .Where(p => p.DriverId == driver.Id && p.PickupDay == today)
                .ToListAsync();

            var statusList = await _context.DriverPickupStatuses
     .Where(r => r.DriverId == driver.Id)
     .ToListAsync();

            var model = schedules.Select(s => new PickupScheduleViewModel
            {
                ScheduleId = s.Id,
                WardNumber = s.Ward.No,
                PickupTime = s.PickupTime,
                IsCompleted = statusList.Any(r => r.ScheduleId == s.Id)
            }).ToList();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePickupCompletion(List<int> scheduleIds, List<int> completedScheduleIds)
        {
            var userId = _userManager.GetUserId(User);
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
                return NotFound("Driver not found.");

            completedScheduleIds ??= new List<int>();

            foreach (var scheduleId in scheduleIds)
            {
                var isCompleted = completedScheduleIds.Contains(scheduleId);

                var existing = await _context.DriverPickupStatuses
                    .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId && s.DriverId == driver.Id);

                if (isCompleted)
                {
                    if (existing == null)
                    {
                        _context.DriverPickupStatuses.Add(new DriverPickupStatus
                        {
                            ScheduleId = scheduleId,
                            DriverId = driver.Id,
                            CompletedOn = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        existing.CompletedOn = DateTime.UtcNow;
                        _context.DriverPickupStatuses.Update(existing);
                    }
                }
                else
                {
                    if (existing != null)
                        _context.DriverPickupStatuses.Remove(existing);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("PickupChecklist");
        }





        public async Task<IActionResult> Checklist()
        {
            
            var userId = _userManager.GetUserId(User);

           
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);
            if (driver == null)
                return NotFound("Driver not found.");

            var today = DateTime.Today.DayOfWeek;

            
            var scheduledWards = await _context.PickupSchedules
                .Where(ps => ps.DriverId == driver.Id && ps.PickupDay == today)
                .Select(ps => ps.WardId)
                .ToListAsync();

            
            var reports = await _context.Reports
                .Where(r => scheduledWards.Contains(r.WardId) && r.Status == "Pending")
                .ToListAsync();

            
            var statuses = await _context.ReportPickupStatuses
                .Where(s => s.DriverId == driver.Id)
                .ToListAsync();

            var model = reports.Select(r =>
            {
                var status = statuses.FirstOrDefault(s => s.ReportId == r.Id);
                return new ReportViewModel
                {
                    ReportId = r.Id,
                    Description = r.Description,
                    IsPickedUp = status?.IsPickedUp ?? false
                };
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePickupStatus(int reportId, bool isPickedUp)
        {
            var userId = _userManager.GetUserId(User);
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);
            if (driver == null) return NotFound("Driver not found.");

            var existing = await _context.ReportPickupStatuses
                .FirstOrDefaultAsync(p => p.ReportId == reportId && p.DriverId == driver.Id);

            if (existing != null)
            {
                existing.IsPickedUp = isPickedUp;
                existing.PickupTime = isPickedUp ? DateTime.UtcNow : null;
            }
            else
            {
                _context.ReportPickupStatuses.Add(new ReportPickupStatus
                {
                    ReportId = reportId,
                    DriverId = driver.Id,
                    IsPickedUp = isPickedUp,
                    PickupTime = isPickedUp ? DateTime.UtcNow : null
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Checklist");
        }
    }

}
