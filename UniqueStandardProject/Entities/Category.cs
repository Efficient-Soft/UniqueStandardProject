using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace UniqueStandardProject.Entities
{
    [Table("Category")]
    public partial class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [StringLength(100)]
        public string CategoryName { get; set; }
    }
}
