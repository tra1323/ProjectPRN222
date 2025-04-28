using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProjectPRN222.Pages.AdminSite.Notifications
{
    [Authorize(Roles = "User,Staff")]
    public class IndexModel : PageModel
    {
        private readonly Prn222projectContext _context;

        public IndexModel(Prn222projectContext context)
        {
            _context = context;
        }

        public List<Notification> Notifications { get; set; } = new();

        public async Task OnGetAsync()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // L?y Id c?a ng??i dùng hi?n t?i

            Notifications = await _context.Notifications
                .Where(n => n.SendTo == currentUserId) // So sánh v?i Id thay vì Name
                .OrderByDescending(n => n.CreateAt)
                .Include(n => n.CreateByNavigation)
                .ToListAsync();
        }

    }
}
