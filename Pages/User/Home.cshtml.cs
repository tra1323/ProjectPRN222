using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.User
{
    public class HomeModel : PageModel
    {
        private readonly Prn222projectContext _context;
        public HomeModel(Prn222projectContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int pageSize = 16;
        public int TotalPages { get; set; }
        public IList<Product> Products { get; set; }
        public async Task OnGetAsync()
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                query = _context.Products.Where(p => p.Name.Contains(Search));
            }

            int totalItems = await query.CountAsync();

            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (CurrentPage <= 0 || CurrentPage > TotalPages)
            {
                CurrentPage = 1;
            }
            Products = await query
                            .Skip((CurrentPage - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        }
    }
}
