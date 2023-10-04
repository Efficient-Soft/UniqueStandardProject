using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace UniqueStandardProject.Entities
{
    [Table("Product")]
    public partial class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Column("Product")]
        [StringLength(500)]
        public string Product1 { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdateDate { get; set; }
        [StringLength(256)]
        public string UpdateUser { get; set; }
        public int? SortOrder { get; set; }
    }
}
