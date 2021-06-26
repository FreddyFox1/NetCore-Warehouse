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
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _db;

        public RegisterModel(
            UserManager<WarehouseUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<WarehouseUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
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
            public string UserName { get; set; }

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
            if (ModelState.IsValid)//&& (role != "Нет роли" || role != String.Empty))
            {
                var user = new WarehouseUser { UserName = Input.UserName, Email = Input.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    _logger.LogInformation("User created a new account with password.");
                    ChangeName(user);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    //}
                    //else
                    //{
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                    //}
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            var roles = _roleManager.Roles.ToList();
            Staff = new SelectList(roles);
            return Page();
        }

        private async void ChangeName(WarehouseUser user)
        {
            string _NormalizeName = await _userManager.GetEmailAsync(user);
            var _user = _db.Users.FirstOrDefault(a => a.Id == user.Id);
            _user.NormalizedUserName = _NormalizeName;
            await _db.SaveChangesAsync();
        }
    }
}