
using LandmarkHunt.Areas.Identity.Data;
using LandmarkHunt.Constants;
using Microsoft.AspNetCore.Identity;

namespace LandmarkHunt.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<AppUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            if (roleManager != null)
            {
                //If it throws an error here, try udpating the database in the Packege Manager Console with "Update-Database"
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Mod.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }
            var user = new AppUser
            {

                UserName = "itkariera202223@gmail.com",
                Email = "itkariera202223@gmail.com"
     
            };
            if (userManager != null)
            {
                
                var userInDb = await userManager.FindByEmailAsync(user.Email);
                if (userInDb == null)
                {
                    user.EmailConfirmed = true;
                    await userManager.CreateAsync(user, "Itkariera2023_");
                   // await userManager.AddToRoleAsync(user, Roles.Admin.ToString());


                }
                userInDb = await userManager.FindByEmailAsync(user.Email);
                await userManager.AddToRoleAsync(userInDb!, Roles.Admin.ToString());
            }
        }
    }
}


