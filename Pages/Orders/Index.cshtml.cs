using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ProjectPRN222.Pages.Orders
{
    [Authorize(Roles = "Staff")]
    public class IndexModel : PageModel
    {
        private readonly Prn222projectContext _context;

        public IndexModel(Prn222projectContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; } // Tìm ki?m theo mã ??n hàng ho?c tên ng??i dùng

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; } // L?c theo tr?ng thái ??n hàng

        public async Task OnGetAsync()
        {
            var query = _context.Orders
                .Include(o => o.User)
                .AsQueryable();

            // Tìm ki?m theo mã ??n hàng ho?c tên ng??i dùng
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(o => o.Id.ToString().Contains(SearchTerm) ||
                                         o.User.UserName.Contains(SearchTerm));
            }

            // L?c theo tr?ng thái ??n hàng
            if (!string.IsNullOrEmpty(StatusFilter))
            {
                query = query.Where(o => o.Status == StatusFilter);
            }

            Orders = await query
                .OrderByDescending(o => o.CreateAt)
                .ToListAsync();
        }
    }
}
