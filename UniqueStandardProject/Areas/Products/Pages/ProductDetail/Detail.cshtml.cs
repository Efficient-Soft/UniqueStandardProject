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

        public List<string> ImageList { get; set; }

        public RelatedModel relatedModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int detailId)
        {
            Entities.ProductDetail productDetail = await _context.ProductDetails.FirstOrDefaultAsync(p => p.DetailId == detailId);

            if (!string.IsNullOrEmpty(productDetail.RelatedId))
            {
                string input = productDetail.RelatedId;
                string[] values = input.Split(',');

                ImageList = new List<string>();

                foreach (var id in values)
                {
                    var relatedImage = _context.ProductDetails.FirstOrDefault(p => p.DetailId == Convert.ToInt32(id)) .Img;
                    ImageList.Add(relatedImage);
                    
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
                    DetailId = productDetail.DetailId,
                    Image = productDetail.Img,
                    Product = _context.Products.FirstOrDefault(p => p.ProductId == productDetail.ProductId).Product1,
                    title = productDetail.Title,
                    Description = Regex.Replace(productDetail.Description, "<.*?>", String.Empty)
                };
            }

            return Page();
        }
    }
}
