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
        public string TargetEmail { get; set; } // N?u g?i 1 ng??i th� ?i?n email

        public List<User> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // L?y th�ng tin ng??i t?o th�ng b�o
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                TempData["Error"] = "Ng??i d�ng hi?n t?i kh�ng t?n t?i.";
                return RedirectToPage();
            }

            Notification.CreateAt = DateTime.Now;
            Notification.CreateBy = currentUser.Id; // S? d?ng Id thay v� Name

            if (SelectedOption == "single")
            {
                // Ki?m tra email m?c ti�u
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == TargetEmail);
                if (user == null)
                {
                    TempData["Error"] = "Kh�ng t�m th?y ng??i d�ng v?i email n�y.";
                    return RedirectToPage();
                }

                Notification.SendTo = user.Id; // G�n Id c?a ng??i nh?n
                _context.Notifications.Add(Notification);
            }
            else if (SelectedOption == "all")
            {
                // L?y danh s�ch ng??i d�ng trong c�c vai tr� "User" v� "Staff"
                var usersInRoles = new List<User>();
                foreach (var roleName in new[] { "User", "Staff" })
                {
                    var users = await _userManager.GetUsersInRoleAsync(roleName);
                    usersInRoles.AddRange(users);
                }

                var distinctUsers = usersInRoles.DistinctBy(u => u.Id); // Lo?i b? tr�ng l?p

                foreach (var user in distinctUsers)
                {
                    var newNoti = new Notification
                    {
                        Title = Notification.Title,
                        CreateBy = currentUser.Id, // G�n Id c?a ng??i t?o
                        CreateAt = Notification.CreateAt,
                        SendTo = user.Id // G�n Id c?a ng??i nh?n
                    };
                    _context.Notifications.Add(newNoti);
                }
            }

            // L?u thay ??i v�o c? s? d? li?u
            await _context.SaveChangesAsync();
            TempData["Success"] = "G?i th�ng b�o th�nh c�ng.";
            return RedirectToPage();
        }

    }
}
