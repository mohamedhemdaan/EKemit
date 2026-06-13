using EKemit.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKemit.Repository.Identity
{
    //public static class AppIdentityDbContextSeed
    //{
    //    public static async Task SeedUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    //    {
    //        // Ensure the roles exist
    //        if (!roleManager.Roles.Any())
    //        {
    //            await roleManager.CreateAsync(new IdentityRole("Admin"));
    //            await roleManager.CreateAsync(new IdentityRole("Seller"));
    //            await roleManager.CreateAsync(new IdentityRole("Buyer"));
    //        }

    //        var userEmail = "mohamedhemdan743@gmail.com";
    //        var existingUser = await userManager.FindByEmailAsync(userEmail);

    //        if (existingUser == null)
    //        {
    //            var user = new AppUser()
    //            {
    //                DisplayName = "Mohamed Hemdan",
    //                Email = userEmail,
    //                UserName = "mohamedhemdan743",
    //                PhoneNumber = "01202797500"
    //            };
    //            await userManager.CreateAsync(user, "P@ssW0rd");

    //            // Assign the Admin role to the user
    //            await userManager.AddToRoleAsync(user, "Admin");
    //        }
    //        else
    //        {
    //            // Check if the user already has the Admin role
    //            var roles = await userManager.GetRolesAsync(existingUser);
    //            if (!roles.Contains("Admin"))
    //            {
    //                // Assign the Admin role to the existing user
    //                await userManager.AddToRoleAsync(existingUser, "Admin");
    //            }
    //        }
    //    }
    //}


    //public static class AppIdentityDbContextSeed
    //{
    //    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    //    {
    //        if (!await roleManager.RoleExistsAsync("Admin"))
    //        {
    //            await roleManager.CreateAsync(new IdentityRole("Admin"));
    //        }

    //        if (!await roleManager.RoleExistsAsync("Seller"))
    //        {
    //            await roleManager.CreateAsync(new IdentityRole("Seller"));
    //        }

    //        if (!await roleManager.RoleExistsAsync("Buyer"))
    //        {
    //            await roleManager.CreateAsync(new IdentityRole("Buyer"));
    //        }
    //    }

    //    public static async Task SeedUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    //    {
    //        // Ensure roles exist
    //        await SeedRolesAsync(roleManager);

    //        // Ensure a specific user exists
    //        var userEmail = "mohamedhemdan743@gmail.com";
    //        if (await userManager.FindByEmailAsync(userEmail) == null)
    //        {
    //            var user = new AppUser()
    //            {
    //                DisplayName = "Mohamed Hemdan",
    //                Email = userEmail,
    //                UserName = "mohamedhemdan743",
    //                PhoneNumber = "01202797500"
    //            };
    //            await userManager.CreateAsync(user, "P@ssW0rd");

    //            // Assign the Admin role to the user
    //            await userManager.AddToRoleAsync(user, "Admin");
    //        }
    //    }
    //}

    //public static class AppIdentityDbContextSeed
    //{
    //    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    //    {
    //        if (!await roleManager.RoleExistsAsync("Admin"))
    //        {
    //            await roleManager.CreateAsync(new IdentityRole("Admin"));
    //        }

    //        if (!await roleManager.RoleExistsAsync("Seller"))
    //        {
    //            await roleManager.CreateAsync(new IdentityRole("Seller"));
    //        }

    //        if (!await roleManager.RoleExistsAsync("Buyer"))
    //        {
    //            await roleManager.CreateAsync(new IdentityRole("Buyer"));
    //        }
    //    }

    //    public static async Task SeedUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    //    {
    //        // Ensure roles exist
    //        await SeedRolesAsync(roleManager);

    //        // List of users to be created
    //        var users = new List<(string Email, string DisplayName, string UserName, string Role)>
    //    {
    //        ("mohamedhemdan743@gmail.com", "Mohamed Hemdan", "mohamedhemdan743", "Admin"),
    //        ("AlaaEwais@gmail.com", "Alaa Ewais", "alaaewais", "Admin"),
    //        ("NadaMahmoud@gmail.com", "Nada Mahmoud", "nadamahmoud", "Admin"),
    //        ("HaidyMohsen@gmail.com", "Haidy Mohsen", "haidymohsen", "Admin"),
    //        ("Buyer@gmail.com", "Buyer User", "buyeruser", "Buyer")
    //    };

    //        foreach (var user in users)
    //        {
    //            if (await userManager.FindByEmailAsync(user.Email) == null)
    //            {
    //                var appUser = new AppUser()
    //                {
    //                    DisplayName = user.DisplayName,
    //                    Email = user.Email,
    //                    UserName = user.UserName,
    //                    PhoneNumber = "0123456789" // Replace with actual phone numbers if needed
    //                };
    //                await userManager.CreateAsync(appUser, "P@ssW0rd");
    //                await userManager.AddToRoleAsync(appUser, user.Role);
    //            }
    //        }
    //    }
    //}

    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Seller"))
            {
                await roleManager.CreateAsync(new IdentityRole("Seller"));
            }

            if (!await roleManager.RoleExistsAsync("Buyer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Buyer"));
            }
        }

        public static async Task SeedUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles exist
            await SeedRolesAsync(roleManager);

            // List of users to be created
            var users = new List<(string Email, string DisplayName, string UserName, List<string> Roles)>
            {
                ("mohamedhemdan743@gmail.com", "Mohamed Hemdan", "mohamedhemdan743", new List<string> { "Admin" }),
                ("AlaaEwais@gmail.com", "Alaa Ewais", "alaaewais", new List<string> { "Admin", "Buyer" }),
                ("NadaMahmoud@gmail.com", "Nada Mahmoud", "nadamahmoud", new List<string> { "Admin", "Buyer" }),
                ("HaidyMohsen@gmail.com", "Haidy Mohsen", "haidymohsen", new List<string> { "Admin", "Buyer" }),
                ("Buyer@gmail.com", "Buyer User", "buyeruser", new List<string> { "Buyer" })
            };

            foreach (var user in users)
            {
                var existingUser = await userManager.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    var appUser = new AppUser()
                    {
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                        UserName = user.UserName,
                        PhoneNumber = "0123456789" // Replace with actual phone numbers if needed
                    };
                    await userManager.CreateAsync(appUser, "P@ssW0rd");
                    foreach (var role in user.Roles)
                    {
                        await userManager.AddToRoleAsync(appUser, role);
                    }
                }
                else
                {
                    foreach (var role in user.Roles)
                    {
                        if (!await userManager.IsInRoleAsync(existingUser, role))
                        {
                            await userManager.AddToRoleAsync(existingUser, role);
                        }
                    }
                }
            }
        }
    }

}
