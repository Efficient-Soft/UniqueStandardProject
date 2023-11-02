using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniqueStandardProject.Data;
using UniqueStandardProject.Entities;

namespace UniqueStandardProject.Pages
{
    public class IndexModel : PageModel
    {
    //    private readonly ILogger<IndexModel> _logger;
    //    private readonly UserManager<IdentityUser> _userManager;
    //    private readonly SignInManager<IdentityUser> _signInManager;
    //    private readonly UniqueStandardDbContext _context;
    //    private readonly IHttpContextAccessor _accessor;

    //    public IndexModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
    //        IHttpContextAccessor accessor, UniqueStandardDbContext context, ILogger<IndexModel> logger)
    //    {
    //        _userManager = userManager;
    //        _signInManager = signInManager;
    //        _context = context;
    //        _logger = logger;
    //        _accessor = accessor;
    //    }

    //    [BindProperty]
    //    public InputModel Input { get; set; }

    //    public IList<AuthenticationScheme> ExternalLogins { get; set; }
    //    public string ReturnUrl { get; set; }

    //    [TempData]
    //    public string ErrorMessage { get; set; }

    //    public class InputModel
    //    {
    //        [Required]
    //        public string UserName { get; set; }

    //        [Required]
    //        [DataType(DataType.Password)]
    //        public string Password { get; set; }

    //        [Display(Name = "Remember me")]
    //        public bool RememberMe { get; set; }
    //    }

    //    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    //    {

    //        returnUrl = returnUrl ?? Url.Content("~/");

    //        if (ModelState.IsValid)
    //        {
    //            // This doesn't count login failures towards account lockout
    //            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
    //            var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe,false);
    //            if (result.Succeeded)
    //            {
    //                IdentityUser user = await _signInManager.UserManager.FindByNameAsync(Input.UserName);
    //                if (user != null)
    //                {
    //                    return RedirectToPage("/Dashboard");
    //                }
    //            }
    //            else
    //            {
    //                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    //                return Page();
    //            }
    //        }

    //        ModelState.AddModelError(string.Empty, "Invalid Login attemp.");
    //        return Page();

    //    }

    //    public IActionResult OnGetAsync(string returnUrl = null)
    //    {
    //        //if (!string.IsNullOrEmpty(ErrorMessage))
    //        //{
    //        //    ModelState.AddModelError(string.Empty, ErrorMessage);
    //        //}

    //        //returnUrl ??= Url.Content("~/");

    //        //// Clear the existing external cookie to ensure a clean login process
    //        //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

    //        //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

    //        //ReturnUrl = returnUrl;
    //        return Page();
    //    }
    }
}

