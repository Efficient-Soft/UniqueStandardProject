using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using UniqueStandardProject.Data;
using UniqueStandardProject.Interfaces;

namespace UniqueStandardProject.Areas.Products.Pages.WeldingTraining
{
    public class CreateModel : PageModel
    {
        private readonly UniqueStandardDbContext _context;
        private readonly IFileService _imageService;

        public CreateModel(UniqueStandardDbContext context, IFileService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        [BindProperty]
        public string Base64String_Photo { get; set; }

        [BindProperty]
        public Entities.WeldingTraining WeldingTraining { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Base64String_Photo))
            {
                ModelState.AddModelError("", "Welding Training Image is required");
            }

            Entities.WeldingTraining welding = new()
            {
                WeldingId = WeldingTraining.WeldingId,
                Title = WeldingTraining.Title,
                Description = WeldingTraining.Description,
                SortOrder = WeldingTraining.SortOrder,
                CreateDate = DateTime.Now,
            };

            _context.WeldingTrainings.Add(welding);

            if(await _context.SaveChangesAsync() > 0)
            {
                if (!string.IsNullOrEmpty(Base64String_Photo))
                {
                    byte[] bytes = Convert.FromBase64String(Base64String_Photo.Replace("data:image/jpeg;base64,", string.Empty));
                    MemoryStream ms = new(bytes);
                    Image image = Image.FromStream(ms);
                    ms.Close();
                    if (_imageService.WriteImage(image, $"{welding.WeldingId}", $"{welding.WeldingId}.jpg", "welding"))
                    {
                        var existingWelding = await _context.WeldingTrainings.FirstOrDefaultAsync(w => w.WeldingId ==  welding.WeldingId);  
                        if (existingWelding != null)
                        {
                            welding.Img = $"images/welding/{welding.WeldingId}.jpg";
                            _context.Entry(welding).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }

                }

                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
