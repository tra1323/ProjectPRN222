using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.User
{
    public class ProductDetails: PageModel
    {
        private readonly Prn222projectContext _context;

        public ProductDetails(Prn222projectContext context)
        {
            _context = context;
        }

        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

    
        public async Task<IActionResult> OnPostAsync(int ProductId)
        {
          
            return RedirectToPage(new { id = ProductId });
        }
    }
}