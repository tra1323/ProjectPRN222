using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectPRN222.Pages.Products
{
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        private readonly Prn222projectContext _context;

        public CreateModel(Prn222projectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }
        public List<Category> Categories { get; set; } = new();
        public List<Manufacture> Manufacturers { get; set; } = new();

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.ToListAsync();
            Manufacturers = await _context.Manufactures.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories.ToListAsync();
                Manufacturers = await _context.Manufactures.ToListAsync();
                return Page();
            }

            // Thêm s?n ph?m m?i
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm s?n ph?m thành công.";
            return RedirectToPage("./Index");
        }
    }
}
