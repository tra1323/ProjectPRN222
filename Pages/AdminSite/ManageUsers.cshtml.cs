using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;
namespace ProjectPRN222.Pages.AdminSite
{
    public class ManageUsersModel : PageModel
    {
        //private readonly Prn222projectContext _context;

        //public ManageUsersModel(Prn222projectContext context)
        //{
        //    _context = context;
        //}

        //public List<User> Users { get; set; } = new List<User>();

        //public async Task OnGetAsync()
        //{
        //    // L?y danh sách ng??i dùng và vai trò c?a h?
        //    Users = await _context.Users
        //                           .Include(u => u.UserRoles)
        //                           .ThenInclude(ur => ur.Role)
        //                           .ToListAsync();
        //}

        //public async Task<IActionResult> OnPostAssignStaffRoleAsync(string userId)
        //{
        //    var user = await _context.Users
        //                             .Include(u => u.UserRoles)
        //                             .ThenInclude(ur => ur.Role)
        //                             .FirstOrDefaultAsync(u => u.Id == userId);

        //    if (user != null)
        //    {
        //        var staffRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Staff");

        //        if (staffRole != null && !user.UserRoles.Any(ur => ur.RoleId == staffRole.Id))
        //        {
        //            _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = staffRole.Id });
        //            await _context.SaveChangesAsync();
        //        }
        //    }

        //    return RedirectToPage();  // Refresh page after operation
        //}

        //public async Task<IActionResult> OnPostRemoveStaffRoleAsync(string userId)
        //{
        //    var user = await _context.Users
        //                             .Include(u => u.UserRoles)
        //                             .ThenInclude(ur => ur.Role)
        //                             .FirstOrDefaultAsync(u => u.Id == userId);

        //    if (user != null)
        //    {
        //        var staffRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Staff");

        //        if (staffRole != null)
        //        {
        //            var userRole = user.UserRoles.FirstOrDefault(ur => ur.RoleId == staffRole.Id);

        //            if (userRole != null)
        //            {
        //                _context.UserRoles.Remove(userRole);
        //                await _context.SaveChangesAsync();
        //            }
        //        }
        //    }

        //    return RedirectToPage();  // Refresh page after operation
        //}
    }
}
