using System.Collections.Generic;
using LandmarkHunt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace LandmarkHunt.Models
{
    
    public class ManageRolesViewModel
    {

        
        private readonly UserManager<AppUser>? _userManager;
        public List<AppUser> Users { get; set; } = new List<AppUser>();
        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
        public List<UserRoleViewModel>? UserRoles { get; set; }

        public ManageRolesViewModel(UserManager<AppUser>? userManager)
        {
            _userManager = userManager;
            UserRoles = new List<UserRoleViewModel>();
        }
        public ManageRolesViewModel()
        {
        }

        public SelectList RoleList
        {
            get
            {
                return new SelectList(Roles, "Name", "Name");
            }
        }

        public async Task<string> GetUserRole(AppUser user)
        {
            if (_userManager != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    return roles.First();
                }

                else
                {
                    return "No Role";
                }
            }
            else throw new Exception("_userManager is null.");
        }
    }

    public class UserRoleViewModel
    {
        public string UserId { get; set; } = "";
        public string RoleName { get; set; } = "";
    }
}
