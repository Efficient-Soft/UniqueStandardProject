using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniqueStandardProject.Data;
using UniqueStandardProject.Interfaces;

namespace UniqueStandardProject.Areas.Products.Pages.Distributor
{
    public class CreateModel : PageModel
    {
        private readonly UniqueStandardDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFileService _imageService;

        public CreateModel(UniqueStandardDbContext context,
                           UserManager<IdentityUser> userManager,
                           IFileService imageService)
        {
            _context = context;
            _userManager = userManager;
            _imageService = imageService;
        }

        [BindProperty]
        public string Base64String_Photo { get; set; }

        [BindProperty]
        public Entities.Distributor Input { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Base64String_Photo))
            {
                ModelState.AddModelError("", "Product Image is required.");
            }

            if (!string.IsNullOrEmpty(Base64String_Photo))
            {
                Input.Img = $"images/distributor/{Input.Id}.jpg";
            }

            Entities.Distributor distributor = new()
            {
                Id = Input.Id,
                Name = Input.Name,
                Img = Input.Img,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateUser = User.Identity.Name,
                SortOrder = Input.SortOrder,
            };

            _context.Distributors.Add(distributor);

            if (await _context.SaveChangesAsync() > 0)
            {
                if (!string.IsNullOrEmpty(Base64String_Photo))
                {
                    byte[] bytes = Convert.FromBase64String(Base64String_Photo.Replace("data:image/jpeg;base64,", string.Empty));
                    MemoryStream ms = new(bytes);
                    Image image = Image.FromStream(ms);
                    ms.Close();
                    _imageService.WriteImage(image, $"{distributor.Id}", $"{distributor.Id}.jpg", "distributor");
                }

                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
