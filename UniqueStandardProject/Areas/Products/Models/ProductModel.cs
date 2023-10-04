using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniqueStandardProject.Areas.Products.Models
{
    public class ProductModel
    {
        public int ImageId { get; set; }
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
    }
}
