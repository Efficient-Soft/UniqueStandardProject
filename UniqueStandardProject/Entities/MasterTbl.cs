using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace UniqueStandardProject.Entities
{
    [Table("MasterTbl")]
    public partial class MasterTbl
    {
        [Key]
        public int Id { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string AboutUs { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(500)]
        public string Phone { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
    }
}
