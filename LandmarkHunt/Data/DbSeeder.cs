
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
                
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }
            var user = new AppUser
            {
<<<<<<< HEAD
                UserName = "itkariera202223@gmail.com",
                Email = "itkariera202223@gmail.com"
             
=======
                UserName = "itkariera2023@gmail.com",
                Email = "itkariera2023@gmail.com"
>>>>>>> 5b0d336147082f75a9c05a7d27c10d331d18d032
            };
            if (userManager != null)
            {
                
                var userInDb = await userManager.FindByEmailAsync(user.Email);
                if (userInDb == null)
                {
                    user.EmailConfirmed = true;
                    await userManager.CreateAsync(user, "Itkariera2023_");
                   // await userManager.AddToRoleAsync(user, Roles.Admin.ToString());

<<<<<<< HEAD
                }
                userInDb = await userManager.FindByEmailAsync(user.Email);
                await userManager.AddToRoleAsync(userInDb!, Roles.Admin.ToString());


=======

                }

                userInDb = await userManager.FindByEmailAsync(user.Email);
                await userManager.AddToRoleAsync(userInDb!, Roles.Admin.ToString());


>>>>>>> 5b0d336147082f75a9c05a7d27c10d331d18d032
            }
        }
    }
}


