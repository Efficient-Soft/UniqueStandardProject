using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace UniqueStandardProject.Entities
{
    [Table("UserInfo")]
    public partial class UserInfo
    {
        [Key]
        [StringLength(256)]
        public string UserId { get; set; }
        [StringLength(200)]
        public string FullName { get; set; }
        [StringLength(500)]
        public string Image { get; set; }
        [StringLength(500)]
        public string DetailAddress { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public bool Status { get; set; }
    }
}
