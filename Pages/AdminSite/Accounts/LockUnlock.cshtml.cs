using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.AdminSite.Accounts
{
    [Authorize(Roles = "Admin")]
    public class LockUnlockModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public LockUnlockModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public User TargetUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            TargetUser = await _userManager.FindByIdAsync(id);
            if (TargetUser == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                // M? kh�a
                user.LockoutEnd = null;
                TempData["Success"] = "?� m? kh�a t�i kho?n.";
            }
            else
            {
                // Kh�a trong 100 n?m
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
                TempData["Success"] = "?� kh�a t�i kho?n.";
            }

            await _userManager.UpdateAsync(user);
            return RedirectToPage("./Index");
        }
    }
}
