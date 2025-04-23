using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.UserSite
{
    public class ProductDetails : PageModel
    {
        private readonly Prn222projectContext _context;
        private readonly UserManager<User> _userManager;

        public ProductDetails(Prn222projectContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            _context.Cards.Add(new Card(user.Id, productId, 1));
            await _context.SaveChangesAsync();
            return Page();
        }
    }
}