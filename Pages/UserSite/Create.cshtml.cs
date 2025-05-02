using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectPRN222.Models;

namespace ProjectPRN222.Pages.UserSite
{
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        private readonly Prn222projectContext _context;
        private readonly IWebHostEnvironment _environment;
        public CreateModel(Prn222projectContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            LoadDropdowns();
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        [BindProperty]
        public IFormFile? ImageFile { get; set; }



        public List<SelectListItem> CategoryOptions { get; set; } = new();
        public List<SelectListItem> ManufacturerOptions { get; set; } = new();

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return Page();
            }




            if (ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                Product.ImageUrl = "/images/" + fileName;
            }
            Product.isActive = true;
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index1");
        }

        private void LoadDropdowns()
        {
            CategoryOptions = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            ManufacturerOptions = _context.Manufactures
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList();
        }
    }
}