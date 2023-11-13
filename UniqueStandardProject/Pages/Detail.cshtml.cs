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
        public IActionResult OnGet(int detailId , string title, string description)
        {
            string desc = Regex.Match(description, @"<p>(.*?)</p>").Groups[1].Value;
            ViewData["Id"] = detailId;
            ViewData["title"] = title;
            ViewData["description"] = desc; 

            return Page();
        }
    }
}
