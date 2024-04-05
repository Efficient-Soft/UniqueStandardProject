using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using UniqueStandardProject.Data;
using UniqueStandardProject.Interfaces;

namespace UniqueStandardProject.Areas.Products.Pages.Activity
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
        public Entities.Activity Activity { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Base64String_Photo))
            {
                ModelState.AddModelError("","Activity Image is required.");
            }

            Entities.Activity activity = new()
            {
                ActivityId = Activity.ActivityId,
                Title = Activity.Title,
                Img = Activity.Img,
                CreateDate = DateTime.Now,
                SortOrder = Activity.SortOrder
            };

            _context.Activities.Add(activity);

            if(await _context.SaveChangesAsync() > 0)
            {
                if(!string.IsNullOrEmpty(Base64String_Photo))
                {
                    byte[] bytes = Convert.FromBase64String(Base64String_Photo.Replace("data:image/jpeg;base64,",string.Empty));
                    MemoryStream ms = new (bytes);
                    Image image = Image.FromStream(ms);
                    ms.Close();
                    if (_imageService.WriteImage(image, $"{activity.ActivityId}", $"{activity.ActivityId}.jpg", "activity"))
                    {
                        var existingActivity = await _context.Activities.FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId);
                        if(existingActivity != null)
                        {
                            activity.Img = $"images/activity/{existingActivity.ActivityId}.jpg";
                            _context.Entry(existingActivity).State = EntityState.Modified;
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
