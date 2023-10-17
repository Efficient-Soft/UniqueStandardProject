using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniqueStandardProject.Areas.Products.Models
{
    public class EditProductModel
    {
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public int ServiceId { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public string Desc { get; set; }
        public int? SortOrder { get; set; }
    }
}
