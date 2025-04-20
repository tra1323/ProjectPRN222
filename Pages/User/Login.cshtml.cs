using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly Prn222projectContext _context;
        public LoginModel(Prn222projectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginInput Input { get; set; }

        public string ErrorMessage { get; set; }

        public class LoginInput
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public IActionResult OnGet()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
         

            var user = _context.Users
                        .FirstOrDefault(u => u.Email == Input.Email && u.Password == Input.Password); // Bạn nên mã hoá mật khẩu ở thực tế

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);

                return RedirectToPage("Index");
            }
            else
            {
                ErrorMessage = "Email hoặc mật khẩu không đúng.";
                return Page();
            }
        }
    }
}
