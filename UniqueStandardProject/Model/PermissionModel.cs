using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFoodWeb.Model
{

    public class PermissionModel
    {
        public string Name { get; set; }
        public string Area { get; set; }
        public List<PolicyModel> Policies { get; set; }
    }

    public class PolicyModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string Policy { get; set; }
        public string Claim_Type { get; set; }
    }
}
