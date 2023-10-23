using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniqueStandardProject.Entities;

namespace UniqueStandardProject.Areas.Products.Models
{
    public class ImageModel
    {
        public List<relatedImage> Images;
    }

    public class relatedImage
    {
        public string ImgTitle { get; set; }
        public string Img { get; set; }
    }

}
