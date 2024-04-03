using Microsoft.AspNetCore.Http;

namespace UniqueStandardProject.Areas.Products.Models
{
    public class EditActivityModel
    {
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public int SortOrder { get; set; }
    }
}
