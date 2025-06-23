using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using Trash_Track.Models;
using Trash_Track.Models.ViewModels;
namespace Trash_Track.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly TrashDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserController(TrashDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: /User/
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var users = await _context.ApplicationUsers.ToListAsync();
            var viewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                viewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    ContactNumber = user.ContactNumber,
                    Address = user.Address,
                    WardNumber = user.WardNumber,
                    Role = role ?? "Unknown",
                    IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow
                });
            }
            return View(viewModels);
        }
        // POST: /User/Lock
        [HttpPost]
        public async Task<IActionResult> Lock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.LockoutEnd = DateTime.UtcNow.AddYears(100); // Effectively permanent lock
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }
        // POST: /User/Unlock
        [HttpPost]
        public async Task<IActionResult> Unlock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.LockoutEnd = null;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}