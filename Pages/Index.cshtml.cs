using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<User> _userManager;

        public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public User CurrentUser { get; set; }
        public IList<string> roles { get; set; }
        public async Task OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            roles = await _userManager.GetRolesAsync(CurrentUser);
            foreach (var role in roles)
            {
                Console.WriteLine(role);  
            }

        }
    }
}
