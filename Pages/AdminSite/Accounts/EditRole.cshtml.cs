using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.AdminSite.Accounts
{
    [Authorize(Roles = "Admin")]
    public class EditRoleModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditRoleModel(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public string SelectedRole { get; set; }

        public User TargetUser { get; set; }

        public List<string> AllRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            TargetUser = await _userManager.FindByIdAsync(id);
            if (TargetUser == null) return NotFound();

            AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var userRoles = await _userManager.GetRolesAsync(TargetUser);
            SelectedRole = userRoles.FirstOrDefault();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles); // X�a t?t c? role hi?n t?i
            await _userManager.AddToRoleAsync(user, SelectedRole); // Th�m role m?i

            TempData["Success"] = "?� c?p nh?t vai tr�.";
            return RedirectToPage("./Index");
        }
    }
}
