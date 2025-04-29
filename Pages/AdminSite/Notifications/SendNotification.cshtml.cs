using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ProjectPRN222.Pages.AdminSite.Notifications
{
    [Authorize(Roles = "Admin")]
    public class SendNotificationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Prn222projectContext _context;

        public SendNotificationModel(Prn222projectContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public Notification Notification { get; set; }

        [BindProperty]
        public string SelectedOption { get; set; } // L?a ch?n: "single" ho?c "all"

        [BindProperty]
        public string TargetEmail { get; set; } // N?u g?i 1 ng??i thì ?i?n email

        public List<User> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // L?y thông tin ng??i t?o thông báo
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                TempData["Error"] = "Ng??i dùng hi?n t?i không t?n t?i.";
                return RedirectToPage();
            }

            Notification.CreateAt = DateTime.Now;
            Notification.CreateBy = currentUser.Id; // S? d?ng Id thay vì Name

            if (SelectedOption == "single")
            {
                // Ki?m tra email m?c tiêu
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == TargetEmail);
                if (user == null)
                {
                    TempData["Error"] = "Không tìm th?y ng??i dùng v?i email này.";
                    return RedirectToPage();
                }

                Notification.SendTo = user.Id; // Gán Id c?a ng??i nh?n
                _context.Notifications.Add(Notification);
            }
            else if (SelectedOption == "all")
            {
                // L?y danh sách ng??i dùng trong các vai trò "User" và "Staff"
                var usersInRoles = new List<User>();
                foreach (var roleName in new[] { "User", "Staff" })
                {
                    var users = await _userManager.GetUsersInRoleAsync(roleName);
                    usersInRoles.AddRange(users);
                }

                var distinctUsers = usersInRoles.DistinctBy(u => u.Id); // Lo?i b? trùng l?p

                foreach (var user in distinctUsers)
                {
                    var newNoti = new Notification
                    {
                        Title = Notification.Title,
                        CreateBy = currentUser.Id, // Gán Id c?a ng??i t?o
                        CreateAt = Notification.CreateAt,
                        SendTo = user.Id // Gán Id c?a ng??i nh?n
                    };
                    _context.Notifications.Add(newNoti);
                }
            }

            // L?u thay ??i vào c? s? d? li?u
            await _context.SaveChangesAsync();
            TempData["Success"] = "G?i thông báo thành công.";
            return RedirectToPage();
        }

    }
}
