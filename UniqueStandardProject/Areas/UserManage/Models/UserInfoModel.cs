using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniqueStandardProject.Areas.UserManage.Models
{
    public class UserInfoModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string DetailAddress { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
