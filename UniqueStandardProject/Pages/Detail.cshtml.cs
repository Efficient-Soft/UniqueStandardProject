using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Text.RegularExpressions;
using System.Threading.Tasks;

using UniqueStandardProject.Data;

namespace UniqueStandardProject.Pages
{
    public class DetailModel : PageModel
    {
        private readonly UniqueStandardDbContext _context;
        public DetailModel(UniqueStandardDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int detailId, string title, string description)
        {
            ViewData["Id"] = detailId;
            ViewData["title"] = title;

            Entities.ProductDetail productDetail = await _context.ProductDetails.FirstOrDefaultAsync(x => x.DetailId == detailId);

            ViewData["description"] = productDetail.Description;

            return Page();
        }
    }
}
