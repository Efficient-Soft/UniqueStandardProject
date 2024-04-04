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

namespace UniqueStandardProject.Areas.Products.Pages.ProductDetail
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
        public Entities.ProductDetail Input { get; set; }
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



            Entities.ProductDetail productDetail = new()
            {
                ProductId = Input.ProductId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateUser = User.Identity.Name,
                Description = Input.Description,
                Img = Input.Img,
                SortOrder = Input.SortOrder,
                Title = Input.Title
            };
            _context.ProductDetails.Add(productDetail);

            if (await _context.SaveChangesAsync() > 0)
            {
                if (!string.IsNullOrEmpty(Base64String_Photo))
                {
                    byte[] bytes = Convert.FromBase64String(Base64String_Photo.Replace("data:image/jpeg;base64,", string.Empty));
                    MemoryStream ms = new(bytes);
                    Image image = Image.FromStream(ms);
                    ms.Close();
                    if (_imageService.WriteImage(image, $"{productDetail.DetailId}-{Input.ProductId}", $"{productDetail.DetailId}-{Input.ProductId}.jpg", "productDetail"))
                    {
                        var existingProductDetail = await _context.ProductDetails.FirstOrDefaultAsync(x => x.DetailId == productDetail.DetailId && x.ProductId == productDetail.ProductId);
                        if (existingProductDetail != null)
                        {
                            existingProductDetail.Img = $"images/productDetail/{existingProductDetail.DetailId}-{existingProductDetail.ProductId}.jpg";
                            _context.Entry(existingProductDetail).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    return RedirectToPage("./Index");
                }
                
            }
            return Page();
        }

    }
}
