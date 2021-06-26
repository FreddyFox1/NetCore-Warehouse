using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Warehouse.Model;

namespace Warehouse.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<WarehouseUser> _userManager;
        private readonly SignInManager<WarehouseUser> _signInManager;

        public IndexModel(
            UserManager<WarehouseUser> userManager,
            SignInManager<WarehouseUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }
        [BindProperty]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(WarehouseUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            Username = userName;
            var userEmail = await _userManager.GetEmailAsync(user);
            Email = userEmail;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            await _userManager.SetUserNameAsync(user, Username);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Данные профиля обновлены";
            return RedirectToPage();
        }
    }
}
