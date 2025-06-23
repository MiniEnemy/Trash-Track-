using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trash_Track.Models;

namespace Trash_Track.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly TrashDBContext _context;

        public ReportController(TrashDBContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            // Build the ward dropdown list
            var wards = _context.Wards.OrderBy(w => w.No).ToList();
            var wardList = wards.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = $"Ward {w.No} - {w.Name}"
            }).ToList();

            ViewBag.Wards = wardList;
            return View();
        }

        // POST: Report/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Report report, IFormFile? photo)
        {
            // Prevent EF validation of navigation property
            ModelState.Remove("Ward");

            if (ModelState.IsValid)
            {
                // Handle optional photo upload
                if (photo != null)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsDir))
                        Directory.CreateDirectory(uploadsDir);

                    var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                    var filePath = Path.Combine(uploadsDir, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await photo.CopyToAsync(stream);

                    report.PhotoPath = "/uploads/" + fileName;
                }

                report.Status = "Pending";
                report.CreatedAt = DateTime.Now;

                _context.Reports.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Rebuild the ward dropdown if validation fails
            var wards = _context.Wards.OrderBy(w => w.No).ToList();
            var wardList = wards.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = $"Ward {w.No} - {w.Name}",
                Selected = w.Id == report.WardId
            }).ToList();

            ViewBag.Wards = wardList;

            return View(report);
        }


        // GET: Report
        public async Task<IActionResult> Index()
        {
            var reports = await _context.Reports
                .Include(r => r.Ward)
                .Include(r => r.AssignedDriver)
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

            // Build ward dropdown
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

        // POST: Report/Edit/5
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

        // GET: Report/Delete/5
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

            return View(report); // Will create Delete.cshtml for confirmation
        }
        // POST: Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            // Optional: Delete photo from server
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
