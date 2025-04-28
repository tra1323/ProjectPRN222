using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.Products
{
    [Authorize(Roles = "Staff")]
    public class IndexModel : PageModel
    {
        private readonly Prn222projectContext _context;

        public IndexModel(Prn222projectContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}
