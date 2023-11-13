using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UniqueStandardProject.Areas.Products.Models;
using UniqueStandardProject.Data;
using UniqueStandardProject.Interfaces;

namespace UniqueStandardProject.Areas.Products.Pages.ProductDetail
{
    public class DetailModel : PageModel
    {
        private readonly UniqueStandardDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFileService _imageService;

        public DetailModel(UniqueStandardDbContext context,
                           UserManager<IdentityUser> userManager,
                           IFileService imageService)
        {
            _context = context;
            _userManager = userManager;
            _imageService = imageService;
        }

        //public List<string> ImageList { get; set; }
        //public List<string> TitleList { get; set; }
        public ImageModel imageModels { get; set; }
        public RelatedModel relatedModel { get; set; }
        public ProductModel ProductModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int detailId)
        {
            Entities.ProductDetail productDetail = await _context.ProductDetails.FirstOrDefaultAsync(p => p.DetailId == detailId);

            if (!string.IsNullOrEmpty(productDetail.RelatedId))
            {
                string input = productDetail.RelatedId;
                string[] values = input.Split(',');
                imageModels = new ImageModel();
                imageModels.Images = new List<relatedImage>();

                for (var id = 0; id < values.Length; id++)
                {

                    var result = _context.ProductDetails.FirstOrDefault(p => p.DetailId == Convert.ToInt32(values[id]));
                    if (result != null)
                    {
                        //var relatedImage = _context.ProductDetails.FirstOrDefault(p => p.DetailId == Convert.ToInt32(id));
                        var relatedImage = _context.ProductDetails
                            .Where(img => img.DetailId == Convert.ToInt32(values[id])).Select(v => new relatedImage()
                            {
                                Img = v.Img,
                                ImgTitle = v.Title
                            }).ToList();

                        if (relatedImage.Count > 0)
                        {
                            imageModels.Images.AddRange(relatedImage);
                        }
                    }
                    
                }

                relatedModel = new()
                {
                    DetailId = productDetail.DetailId,
                    Image = productDetail.Img,
                    Product = _context.Products.FirstOrDefault(p => p.ProductId == productDetail.ProductId).Product1,
                    title = productDetail.Title,
                    Description = Regex.Replace(productDetail.Description, "<.*?>", String.Empty)
                };

            }
            else
            {
                relatedModel = new()
                {
                    DetailId = detailId,
                    Image = _context.ProductDetails.FirstOrDefault(p => p.DetailId == detailId).Img,
                    Product = _context.Products.FirstOrDefault(p => p.ProductId == productDetail.ProductId).Product1,
                    title = _context.ProductDetails.FirstOrDefault(p => p.DetailId == detailId).Title
                    //Description = Regex.Replace(productDetail.Description, "<.*?>", String.Empty)
                };

                imageModels = null;

            }

            return Page();
        }
    }
}
