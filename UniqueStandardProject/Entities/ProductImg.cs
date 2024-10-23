using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace UniqueStandardProject.Entities
{
    [Table("ProductImg")]
    public partial class ProductImg
    {
        [Key]
        public int ImageId { get; set; }
        [Key]
        public int DetailId { get; set; }
        [StringLength(500)]
        public string Img { get; set; }
        public string Title { get; set; }
        public int? SortOrder { get; set; }
    }
}
