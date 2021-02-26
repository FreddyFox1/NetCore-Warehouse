using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Areas.Identity.Data;

namespace Warehouse.Pages.Items
{
    public class IndexModel : PageModel
    {
        private readonly Warehouse.Areas.Identity.Data.ApplicationDbContext _context;

        public IndexModel(Warehouse.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Item> Item { get;set; }

        public async Task OnGetAsync()
        {
            Item = await _context.Items
                .Include(i => i.Category).ToListAsync();
        }
    }
}
