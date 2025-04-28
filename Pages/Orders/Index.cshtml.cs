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
        public string? SearchTerm { get; set; } // T�m ki?m theo m� ??n h�ng ho?c t�n ng??i d�ng

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; } // L?c theo tr?ng th�i ??n h�ng

        public async Task OnGetAsync()
        {
            var query = _context.Orders
                .Include(o => o.User)
                .AsQueryable();

            // T�m ki?m theo m� ??n h�ng ho?c t�n ng??i d�ng
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(o => o.Id.ToString().Contains(SearchTerm) ||
                                         o.User.UserName.Contains(SearchTerm));
            }

            // L?c theo tr?ng th�i ??n h�ng
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
