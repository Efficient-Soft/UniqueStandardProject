using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniqueStandardProject.Entities;

namespace UniqueStandardProject.Pages
{
    public class ServiceModel : PageModel
    {
        public void OnGet(string serviceTitle)
        {
            ViewData["ServiceTitle"] = serviceTitle;
        }
    }
}
