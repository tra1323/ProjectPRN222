using Microsoft.AspNetCore.Identity;
using ProjectPRN222.Models;
using System.Data;

namespace ProjectPRN222.Areas.Identity.SeedData
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Khởi tạo vai trò từ Enum
            await CreateRoles(roleManager);

            // Tạo người dùng nếu chưa có
            await CreateAdminUser(userManager);
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            // Lấy tất cả giá trị từ Enum Role
            foreach (var roleName in Enum.GetNames(typeof(Role)))
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new IdentityRole(roleName);
                    await roleManager.CreateAsync(role);
                }
            }
        }

        private static async Task CreateAdminUser(UserManager<User> userManager)
        {
            var user = await userManager.FindByEmailAsync("admin@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                };
                var result = await userManager.CreateAsync(user, "Admin123!");

                if (result.Succeeded)
                {
                    // Gán vai trò "Admin" cho người dùng
                    await userManager.AddToRoleAsync(user, Role.Admin.ToString());
                }
            }
        }
    }

}
