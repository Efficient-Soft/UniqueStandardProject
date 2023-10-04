using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniqueStandardProject.Entities;

namespace UniqueStandardProject.Areas.Products.Models
{
    public class ProductImgModel
    {
        public int DetailId { get; set; }
        public string Product { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public List<ProductImg> Images { get; set; }
        public List<int> SelectedImages { get; set; }

    }
}
