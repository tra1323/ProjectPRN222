using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.Products
{
    [Authorize(Roles = "Staff")]
    public class DeleteModel : PageModel
    {
        private readonly Prn222projectContext _context;

        public DeleteModel(Prn222projectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Xóa s?n ph?m thành công.";
            return RedirectToPage("./Index");
        }
    }
}
