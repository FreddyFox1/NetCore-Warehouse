using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = "AdminArea")]
    public class AdminModel : PageModel
    {
        private readonly UserManager<WarehouseUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminModel(UserManager<WarehouseUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IList<WarehouseUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userManager.Users.ToListAsync();
        }
    }
}
