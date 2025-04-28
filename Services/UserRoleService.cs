using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;
namespace ProjectPRN222.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly Prn222projectContext _context;

        public UserRoleService(Prn222projectContext context)
        {
            _context = context;
        }

        public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

           // if (user != null && role != null)
           // {
           //     var userRole = new UserRole
           //     {
           //         UserId = user.Id,
           //         RoleId = role.Id
           //     };

           //   _context.UserRoles.Add(userRole);
           //    await _context.SaveChangesAsync();
           //    return true;
           //}

          return false;
       }

       public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleName)
       {
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (user != null && role != null)
            {
                var userRole = await _context.UserRoles
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == role.Id);

                if (userRole != null)
               {
                   _context.UserRoles.Remove(userRole);
                   await _context.SaveChangesAsync();
                   return true;
                }
            }

            return false;
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (user != null && role != null)
            {
                return await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == role.Id);
           }

           return false;
       }
    }
}
