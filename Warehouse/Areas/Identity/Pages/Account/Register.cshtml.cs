using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Warehouse.Model;

namespace Warehouse.Areas.Identity.Pages.Account
{
    [Authorize(Policy = "AdminArea")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<WarehouseUser> _signInManager;
        private readonly UserManager<WarehouseUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _db;

        public RegisterModel(
            UserManager<WarehouseUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<WarehouseUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _db = db;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public SelectList Staff { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Display(Name = "Имя пользователя")]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Пароль {0} должен быть не менее {2} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтверждение пароля")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var roles = _roleManager.Roles.ToList();
            Staff = new SelectList(roles);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var role = Request.Form["SelectedRole"];
            returnUrl = returnUrl ?? Url.Content("~/Admin/Index");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new WarehouseUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true, Name = Input.Name };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    _logger.LogInformation("User created a new account with password.");
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var roles = _roleManager.Roles.ToList();
            Staff = new SelectList(roles);
            return Page();
        }
    }
}