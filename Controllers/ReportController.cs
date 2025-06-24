using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trash_Track.Models;
using Trash_Track.Utility;

namespace Trash_Track.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly TrashDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public ReportController(TrashDBContext context,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);

            int wardId = 0;
            string wardDisplay = "Unknown";

            if (user != null)
            {
                
                var matchedWard = await _context.Wards.FirstOrDefaultAsync(w => w.No == user.WardNumber);
                if (matchedWard != null)
                {
                    wardId = matchedWard.Id;
                    wardDisplay = $"Ward {matchedWard.No} - {matchedWard.Name}";
                }
            }

            var viewModel = new ReportFormViewModel
            {
                ReporterName = user?.FullName ?? user?.UserName ?? "Unknown",
                WardId = wardId
            };

            ViewBag.WardDisplay = wardDisplay;

            ViewBag.Wards = await _context.Wards
                .OrderBy(w => w.No)
                .Select(w => new SelectListItem
                {
                    Value = w.Id.ToString(),
                    Text = $"Ward {w.No} - {w.Name}"
                })
                .ToListAsync();

            return View(viewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReportFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Wards = _context.Wards.OrderBy(w => w.No).Select(w => new SelectListItem
                {
                    Value = w.Id.ToString(),
                    Text = $"Ward {w.No} - {w.Name}",
                    Selected = w.Id == vm.WardId
                }).ToList();

                return View(vm);
            }

            var report = new Report
            {
                ReporterName = vm.ReporterName,
                ReporterUserId = _userManager.GetUserId(User), 
                WardId = vm.WardId,
                Description = vm.Description,
                Status =ReportStatuses.Pending,
                CreatedAt = DateTime.Now,
                    // ✅ Get assigned driver from the pickup schedule
                AssignedDriverId = _context.PickupSchedules
                        .Where(s => s.WardId == vm.WardId)
                        .Select(s => s.DriverId)
                        .FirstOrDefault()
            };

            
            if (vm.Photo != null)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                var fileName = Guid.NewGuid() + Path.GetExtension(vm.Photo.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await vm.Photo.CopyToAsync(stream);

                report.PhotoPath = "/uploads/" + fileName;
            }

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var reports = await _context.Reports
                .Include(r => r.Ward)
                .Include(r => r.AssignedDriver)
                .Where(r => r.ReporterUserId == userId) //Filter only user's reports
                .ToListAsync();

            return View(reports);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }


            var wardList = _context.Wards.OrderBy(w => w.No)
                .Select(w => new SelectListItem
                {
                    Value = w.Id.ToString(),
                    Text = $"Ward {w.No} - {w.Name}",
                    Selected = w.Id == report.WardId
                }).ToList();

            ViewBag.Wards = wardList;
            return View(report);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Report report, IFormFile? newPhoto)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            // Prevent EF from validating navigation property
            ModelState.Remove("Ward");

            if (!ModelState.IsValid)
            {
                var wardList = _context.Wards.OrderBy(w => w.No)
                    .Select(w => new SelectListItem
                    {
                        Value = w.Id.ToString(),
                        Text = $"Ward {w.No} - {w.Name}",
                        Selected = w.Id == report.WardId
                    }).ToList();

                ViewBag.Wards = wardList;
                return View(report);
            }

            try
            {
                var existingReport = await _context.Reports.FindAsync(id);
                if (existingReport == null)
                    return NotFound();

                // Update fields
                existingReport.ReporterName = report.ReporterName;
                existingReport.Description = report.Description;
                existingReport.WardId = report.WardId;
                existingReport.Status = report.Status;
                existingReport.AssignedDriverId = report.AssignedDriverId;

                // Handle new photo upload if provided
                if (newPhoto != null)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsDir))
                        Directory.CreateDirectory(uploadsDir);

                    var fileName = Guid.NewGuid() + Path.GetExtension(newPhoto.FileName);
                    var filePath = Path.Combine(uploadsDir, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await newPhoto.CopyToAsync(stream);

                    existingReport.PhotoPath = "/uploads/" + fileName;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var report = await _context.Reports
                .Include(r => r.Ward)
                .Include(r => r.AssignedDriver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (report == null)
            {
                return NotFound();
            }

            return View(report); 
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(report.PhotoPath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", report.PhotoPath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
