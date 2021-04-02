using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Pages.Items.Groups
{
    [Authorize(Roles = "User")]
    public class IndexModel : PageModel
    {
        private readonly Warehouse.Model.ApplicationDbContext _context;

        public IndexModel(Warehouse.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ItemCategory> ItemCategory { get;set; }

        public async Task OnGetAsync()
        {
            ItemCategory = await _context.ItemsCategories.ToListAsync();
        }
    }
}
