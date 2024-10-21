using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using UniqueStandardProject.Data;
using UniqueStandardProject.Interfaces;

namespace UniqueStandardProject.Areas.Products.Pages.ServiceTbl
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
        public Entities.ServiceTbl Input { get; set; }
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

            Entities.ServiceTbl service = new()
            {
                ServiceId = Input.ServiceId,
                Title = Input.Title,
                Desctiption = Input.Desctiption,
                Img = Input.Img,
                CreateDate = DateTime.Now,
                UpdateUser = User.Identity.Name,
                SortOrder = Input.SortOrder
            };
            _context.ServiceTbls.Add(service);

            if (await _context.SaveChangesAsync() > 0)
            {
                if (!string.IsNullOrEmpty(Base64String_Photo))
                {
                    byte[] bytes = Convert.FromBase64String(Base64String_Photo.Replace("data:image/jpeg;base64,", string.Empty));
                    MemoryStream ms = new(bytes);
                    Image image = Image.FromStream(ms);
                    ms.Close();
                    if (_imageService.WriteImage(image, $"{service.ServiceId}", $"{service.ServiceId}.jpg", "service"))
                    {
                        var existingService = await _context.ServiceTbls.FirstOrDefaultAsync(x => x.ServiceId == service.ServiceId);
                        if (existingService != null)
                        {
                            existingService.Img = $"images/service/{existingService.ServiceId}.jpg";
                            _context.Entry(existingService).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
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
