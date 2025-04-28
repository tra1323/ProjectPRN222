using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.UserSite
{
    public class CardModel : PageModel
    {
        private readonly Prn222projectContext _context;
        private readonly UserManager<User> _userManager;

        public CardModel(Prn222projectContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public User CurrentUser { get; set; }
        public List<Card> CartProducts { get; set; } = new List<Card>();

        [BindProperty]
        public List<int> SelectedIds { get; set; } = new List<int>();

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            CartProducts = await _context.Cards
                .Where(c => c.UserId == CurrentUser.Id)
                .Include(c => c.Product)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync()
        {
            var card = await _context.Cards.FindAsync(Id);
            if (card == null) return NotFound();

            card.Quantity = Quantity;
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostDeleteSingleAsync(int id)
        {
            var cartItem = await _context.Cards.FindAsync(id);
            if (cartItem != null)
            {
                _context.Cards.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

    }
}
