using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace UniqueStandardProject.Entities
{
    [Table("Activity")]
    public partial class Activity
    {
        [Key]
        public int ActivityId { get; set; }
        [StringLength(500)]
        public string Img { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdateDate { get; set; }
        [StringLength(256)]
        public string UpdateUser { get; set; }
        public int? SortOrder { get; set; }
    }
}
