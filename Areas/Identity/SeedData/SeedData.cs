using Microsoft.AspNetCore.Identity;
using ProjectPRN222.Models;

namespace ProjectPRN222.Areas.Identity.SeedData
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Khởi tạo vai trò từ Enum
            await CreateRoles(roleManager);

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            string adminEmail = configuration["AdminUser:Email"];
            string adminPassword = configuration["AdminUser:Password"];

            // Tạo người dùng nếu chưa có
            await CreateAdminUser(userManager, adminEmail, adminPassword);
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

        private static async Task CreateAdminUser(UserManager<User> userManager, string email, string pass)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {

                user = new User
                {
                    UserName = email,
                    Email = email,
                };
                var result = await userManager.CreateAsync(user, pass);

                if (result.Succeeded)
                {
                    // Gán vai trò "Admin" cho người dùng
                    await userManager.AddToRoleAsync(user, Role.Admin.ToString());
                }
            }
        }
    }

}
