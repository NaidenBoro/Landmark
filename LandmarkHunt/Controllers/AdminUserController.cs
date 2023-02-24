using System.Linq;
using System.Threading.Tasks;
using LandmarkHunt.Areas.Identity.Data;
using LandmarkHunt.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LandmarkHunt.Models;

namespace LandmarkHunt.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
            
            return View("~/Views/AdminUser/ManageRoles.cshtml",model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRoles(ManageRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                    foreach (var userRole in model.UserRoles!)
                {
                    var user = await _userManager.FindByIdAsync(userRole.UserId);
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
                return RedirectToAction("ManageRoles");
            }
            return View("~/Views/AdminUser/ManageRoles.cshtml",model);
        }
    }
}
