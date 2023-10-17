using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using UniqueStandardProject.Areas.Products.Models;
using UniqueStandardProject.Areas.UserManage.Models;
using UniqueStandardProject.Data;
using UniqueStandardProject.Entities;
using UniqueStandardProject.Interfaces;

namespace UniqueStandardProject.Areas.Products.Pages.ProductImg
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
        public Entities.ProductImg Input { get; set; }
        public ProductImgModel productModel { get; set; }
        public Entities.ProductDetail product { get; set; }

        [BindProperty]
        public string Base64String_Photo { get; set; }
       // public List<string> Images { get; set; }

        public async Task<IActionResult> OnGetAsync(int detailId)
        {

            Entities.ProductDetail productDetail = await _context.ProductDetails.FirstOrDefaultAsync(pd => pd.DetailId == detailId);

            if (productDetail == null)
            {
                NotFound();
            }

            var imgs = _context.ProductImgs
               .Where(img => img.DetailId == detailId).Select(v => new ImageList() { 
                   ImageId = v.ImageId,
                   Image = v.Img
               }).ToList();

            if(imgs.Count > 0)
            {
                productModel = new ProductImgModel()
                {
                    ImageId = _context.ProductImgs.FirstOrDefault(pi => pi.DetailId == detailId).ImageId,
                    DetailId = detailId,
                    Product = _context.Products.FirstOrDefault(p => p.ProductId == productDetail.ProductId).Product1,
                    Title = productDetail.Title,
                    ImageLists = imgs
                };
            }
            else
            {
                productModel = new ProductImgModel()
                {
                    //ImageId = _context.ProductImgs.FirstOrDefault(pi => pi.DetailId == detailId).ImageId,
                    DetailId = detailId,
                    Product = _context.Products.FirstOrDefault(p => p.ProductId == productDetail.ProductId).Product1,
                    Title = productDetail.Title
                };

            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int detailId)
        {
            if (string.IsNullOrEmpty(Base64String_Photo))
            {
                ModelState.AddModelError("", "Product Image is required.");
            }

            if (!string.IsNullOrEmpty(Base64String_Photo))
            {
                Input.Img = $"images/productImg/{Input.ImageId}-{detailId}.jpg";
            }

            Entities.ProductImg productImg = new()
            {
                DetailId = detailId,
                Img = Input.Img,
                SortOrder = Input.SortOrder,
            };

            _context.ProductImgs.Add(productImg);

            if (await _context.SaveChangesAsync() > 0)
            {
                if (!string.IsNullOrEmpty(Base64String_Photo))
                {
                    byte[] bytes = Convert.FromBase64String(Base64String_Photo.Replace("data:image/jpeg;base64,", string.Empty));
                    MemoryStream ms = new(bytes);
                    Image image = Image.FromStream(ms);
                    ms.Close();
                    _imageService.WriteImage(image, $"{productImg.ImageId}-{detailId}", $"{Input.ImageId}-{detailId}.jpg", "productImg");
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
