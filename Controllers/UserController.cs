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
    public class UserController : Controller
    {
        private readonly TrashDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(TrashDBContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var users = await _context.ApplicationUsers.ToListAsync();
            var viewModels = new List<UserViewModel>();
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            foreach (var user in users)
            {
                var identityUser = await _userManager.FindByIdAsync(user.Id);
                var role = identityUser != null ? (await _userManager.GetRolesAsync(identityUser)).FirstOrDefault() : null;

                viewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    ContactNumber = user.ContactNumber,
                    Address = user.Address,
                    WardNumber = user.WardNumber,
                    Role = role ?? "Unknown",
                    IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow,
                    AvailableRoles = roles,
                    SelectedRole = role
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
                user.LockoutEnd = DateTime.UtcNow.AddYears(100);
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }

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

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrEmpty(newRole)) return RedirectToAction("Index");

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }
            await _userManager.AddToRoleAsync(user, newRole);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            var vm = new UserProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                ContactNumber = user.ContactNumber,
                WardNumber = user.WardNumber
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel vm)
        {
            // Validate profile fields manually (since they're readonly, we need to ensure they're valid)
            if (string.IsNullOrWhiteSpace(vm.Email) ||
                string.IsNullOrWhiteSpace(vm.FullName) ||
                string.IsNullOrWhiteSpace(vm.Address) ||
                string.IsNullOrWhiteSpace(vm.ContactNumber))
            {
                TempData["error"] = "All profile fields are required.";
                return View(vm);
            }

            // Get the current user
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == vm.Id);
            if (user == null)
            {
                TempData["error"] = "User not found.";
                return RedirectToAction("Profile");
            }

            // Check if password change is requested
            bool passwordChangeRequested = !string.IsNullOrWhiteSpace(vm.CurrentPassword) ||
                                         !string.IsNullOrWhiteSpace(vm.NewPassword) ||
                                         !string.IsNullOrWhiteSpace(vm.ConfirmPassword);

            // Only process password change if requested
            if (passwordChangeRequested)
            {
                // Validate that all password fields are provided
                if (string.IsNullOrWhiteSpace(vm.CurrentPassword) ||
                    string.IsNullOrWhiteSpace(vm.NewPassword) ||
                    string.IsNullOrWhiteSpace(vm.ConfirmPassword))
                {
                    TempData["error"] = "All password fields are required to change password.";
                    return View(vm);
                }

                // Server-side validation for password requirements
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                // Get the Identity user
                var identityUser = await _userManager.FindByIdAsync(vm.Id);
                if (identityUser == null)
                {
                    TempData["error"] = "Identity user not found.";
                    return View(vm);
                }

                // Attempt to change password
                var pwdResult = await _userManager.ChangePasswordAsync(
                    identityUser,
                    vm.CurrentPassword,
                    vm.NewPassword
                );

                if (!pwdResult.Succeeded)
                {
                    var errors = string.Join(", ", pwdResult.Errors.Select(e => e.Description));
                    TempData["error"] = "Password change failed: " + errors;
                    return View(vm);
                }

                // Refresh authentication cookie
                await _signInManager.RefreshSignInAsync(identityUser);
                TempData["success"] = "Password updated successfully!";
            }
            else
            {
                TempData["success"] = "Profile information confirmed.";
            }

            return RedirectToAction("Profile");
        }
    }
}