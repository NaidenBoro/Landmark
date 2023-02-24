using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandmarkHunt.Areas.Identity.Data;
using LandmarkHunt.Constants;
using LandmarkHunt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LandmarkHunt.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<IActionResult> ManageRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();
            var model = new ManageRolesViewModel(_userManager)
            {
                Users = users,
                Roles = roles
            };
            return View("~/Views/AdminUser/ManageRoles.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRoles(ManageRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.UserRoles is not null)
                {
                    foreach (var userRole in model.UserRoles)
                    {
                        var user = await _userManager.FindByIdAsync(userRole.UserId);
                        if (user is null)
                        {
                            continue;
                        }
                        var currentRole = await _userManager.GetRolesAsync(user);
                        if (currentRole.Any())
                        {
                            await _userManager.RemoveFromRoleAsync(user, currentRole.First());
                        }
                        if (!string.IsNullOrEmpty(userRole.RoleName))
                        {
                            await _userManager.AddToRoleAsync(user, userRole.RoleName);
                        }
                    }
                    // Update the UserRoles property after updating the roles for each user
                    model.UserRoles = await GetUserRolesAsync(model.Users);
                }
                return RedirectToAction("ManageRoles");
            }
            // If the model state is not valid, return the view with the current model to display validation errors
            model.Roles = await _roleManager.Roles.ToListAsync();
            return View("~/Views/AdminUser/ManageRoles.cshtml", model);
        }

        // A new method to get the updated roles for each user
        private async Task<List<UserRoleViewModel>> GetUserRolesAsync(List<AppUser> users)
        {
            var userRoles = new List<UserRoleViewModel>();
            foreach (var user in users)
            {
                var userRole = new UserRoleViewModel
                {
                    UserId = user.Id,
                    RoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? ""

                };
                userRoles.Add(userRole);
            }
            return userRoles;
        }

    }
}
