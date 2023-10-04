using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniqueStandardProject.Areas.UserManage.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public dynamic Meta { get; set; }
        public dynamic Data { get; set; }
        public dynamic Error { get; set; }
        public dynamic Message { get; set; }

    }
}
