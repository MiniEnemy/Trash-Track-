using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trash_Track.Models;
using Trash_Track.Models.ViewModels;

namespace Trash_Track.Controllers
{
    [Authorize]
    public class PickupController : Controller
    {
        private readonly TrashDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PickupController(TrashDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> MyPickup()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return Unauthorized();
            }

            var today = DateTime.Today;
            var dayOfWeek = today.DayOfWeek;

            // Get regular schedule
            var schedule = await _context.PickupSchedules
                .Include(p => p.Driver)
                .FirstOrDefaultAsync(s => s.Ward.No == user.WardNumber);

            // Check override for today
            var overrideToday = await _context.PickupOverrides
                .Include(o => o.Driver)
                .Where(o => o.Ward.No == user.WardNumber &&
                            o.StartDate <= today && o.EndDate >= today)
                .FirstOrDefaultAsync();
            var ward = await _context.Wards.FirstOrDefaultAsync(w => w.No == user.WardNumber);

            var viewModel = new PickupViewModel
            {
                WardNumber = user.WardNumber,
                WardName = ward?.Name ?? "",

                RegularDay = schedule?.PickupDay,
                RegularTime = schedule?.PickupTime,
                RegularDriver = schedule?.Driver?.Name,

                IsOverrideToday = overrideToday != null,
                IsCancelled = overrideToday?.IsCancelled ?? false,

                
                OverrideDay = overrideToday?.OverrideDay ?? schedule?.PickupDay,
                OverrideTime = overrideToday?.NewTime ?? schedule?.PickupTime,
                OverrideDriver = overrideToday?.Driver?.Name ?? schedule?.Driver?.Name,
                OverrideMessage = overrideToday?.Message
            };


            return View(viewModel);
        }
    }

}
