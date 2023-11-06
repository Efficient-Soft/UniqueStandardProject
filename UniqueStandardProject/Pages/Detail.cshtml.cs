using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniqueStandardProject.Model;

namespace UniqueStandardProject.Pages
{
    public class DetailModel : PageModel
    {
        public IActionResult OnGet(string title, string description)
        {
            string desc = Regex.Match(description, @"<strong>(.*?)</strong>").Groups[1].Value;
            ViewData["title"] = title;
            ViewData["description"] = desc; 

            return Page();
        }
    }
}
