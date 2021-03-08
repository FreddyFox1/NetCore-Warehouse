using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Pages.admin
{
    [Authorize(Policy = "AdminArea")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<WarehouseUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IndexModel(UserManager<WarehouseUser> userManager, RoleManager<IdentityRole> roleManager)
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

        //[HttpPost]
        //public async Task<IActionResult> OnPostDelete(string ID)
        //{
        //    var User = await _userManager.FindByIdAsync(ID);
        //    if (User != null)
        //    {
        //        await _userManager.DeleteAsync(User);
        //        return RedirectToPage();
        //    }
        //    else return Page();
        //}
    }
}