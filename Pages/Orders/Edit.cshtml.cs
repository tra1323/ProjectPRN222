using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN222.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace ProjectPRN222.Pages.Orders
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
        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var orderInDb = await _context.Orders.FindAsync(Order.Id);
            if (orderInDb == null)
            {
                return NotFound();
            }

            orderInDb.Status = Order.Status; // C?p nh?t tr?ng thái
            await _context.SaveChangesAsync();

            TempData["Success"] = "C?p nh?t tr?ng thái ??n hàng thành công.";
            return RedirectToPage("./Index");
        }
    }
}
