using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.UserSite
{
    public class OrderModel : PageModel
    {
        private readonly Prn222projectContext _context;
        private readonly UserManager<User> _userManager;

        public OrderModel(Prn222projectContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Dictionary<OrderStatus, List<Order>> OrdersByStatus { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.CreateAt)
                .ToListAsync();

            OrdersByStatus = orders
                .GroupBy(o => o.Status)
                .ToDictionary(g => g.Key, g => g.ToList());

            return Page();
        }
    }
}
