using Microsoft.AspNetCore.Http;

namespace UniqueStandardProject.Areas.Products.Models
{
    public class EditWeldingTrainingModel
    {
        public int WeldingId { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public int SortOrder { get; set; }
    }
}
