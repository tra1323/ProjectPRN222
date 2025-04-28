using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.UserSite
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ProfileModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [BindProperty]
        public bool IsPhoneConfirmed { get; set; }
        public string? SuccessMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; } = string.Empty;

            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Phone]
            [Display(Name = "Phone Number")]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Address")]
            public string? Address { get; set; }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            Input = new InputModel
            {
                FullName = user.FullName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
            IsPhoneConfirmed = user.PhoneNumberConfirmed;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!ModelState.IsValid)
            {
                Input = new InputModel
                {
                    FullName = user.FullName ?? "",
                    Email = user.Email ?? "",
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                };
                ModelState.Clear();

                TempData["ErrorMessage"] = "Chỉnh sửa thất bại, vui lòng kiểm tra lại thông tin.";
                return Page();
            }

            user.FullName = Input.FullName;
            user.PhoneNumber = Input.PhoneNumber;
            user.Address = Input.Address;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Chỉnh sửa thành công";
                return RedirectToPage();
            }
            return Page();
        }


    }
}