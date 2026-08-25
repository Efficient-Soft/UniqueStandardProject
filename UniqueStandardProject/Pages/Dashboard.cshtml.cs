using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace UniqueStandardProject.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        // TODO: Restore the Dashboard UI when the dashboard feature is ready.
        public IActionResult OnGet()
        {
            return RedirectToPage("/Admin", new { area = "UserManage" });
        }
    }
}
