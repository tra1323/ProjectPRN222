using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectPRN222.Pages.Products
{
    [Authorize(Roles = "Staff")]
    public class EditModel : PageModel
    {
        private readonly Prn222projectContext _context;

        public EditModel(Prn222projectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }
        public List<Category> Categories { get; set; } = new();
        public List<Manufacture> Manufacturers { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            Categories = await _context.Categories.ToListAsync();
            Manufacturers = await _context.Manufactures.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories.ToListAsync();
                Manufacturers = await _context.Manufactures.ToListAsync();
                return Page();
            }

            // Ki?m tra danh m?c và nhà s?n xu?t
            if (Product.CategoryId.HasValue && !await _context.Categories.AnyAsync(c => c.Id == Product.CategoryId))
            {
                ModelState.AddModelError("Product.CategoryId", "Danh m?c không h?p l?.");
            }

            if (Product.ManufacturerId.HasValue && !await _context.Manufactures.AnyAsync(m => m.Id == Product.ManufacturerId))
            {
                ModelState.AddModelError("Product.ManufacturerId", "Nhà s?n xu?t không h?p l?.");
            }

            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories.ToListAsync();
                Manufacturers = await _context.Manufactures.ToListAsync();
                return Page();
            }

            // Truy v?n th?c th? t? c? s? d? li?u
            var productInDb = await _context.Products.FindAsync(Product.Id);
            if (productInDb == null)
            {
                TempData["Error"] = "S?n ph?m không t?n t?i.";
                return RedirectToPage("./Index");
            }

            // C?p nh?t các thu?c tính
            productInDb.Name = Product.Name;
            productInDb.Price = Product.Price;
            productInDb.Stock = Product.Stock;
            productInDb.Description = Product.Description;
            productInDb.CategoryId = Product.CategoryId;
            productInDb.ManufacturerId = Product.ManufacturerId;

            // L?u thay ??i
            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "C?p nh?t s?n ph?m thành công.";
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Có l?i x?y ra khi l?u d? li?u. Vui lòng th? l?i.");
                Categories = await _context.Categories.ToListAsync();
                Manufacturers = await _context.Manufactures.ToListAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }

    }
}
