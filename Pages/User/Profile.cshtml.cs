using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.User
{
    public class ProfileModel : PageModel
    {
        private readonly Prn222projectContext _context;

        public ProfileModel(Prn222projectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProjectPRN222.Models.User UserProfile { get; set; }

        public IActionResult OnGet()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToPage("/User/Login");

            UserProfile = _context.Users.FirstOrDefault(u => u.Email == email);
            if (UserProfile == null)
                return NotFound();

            return Page();
        }

        public IActionResult OnPost()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToPage("/User/Login");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            

            // Cập nhật thông tin từ form
            user.FullName = UserProfile.FullName;
            user.Phone = UserProfile.Phone;

            _context.SaveChanges();

            return RedirectToPage("/User/Profile");
        }
    }
}