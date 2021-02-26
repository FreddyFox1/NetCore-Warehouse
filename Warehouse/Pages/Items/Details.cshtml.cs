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
    public class DetailsModel : PageModel
    {
        private readonly Warehouse.Areas.Identity.Data.ApplicationDbContext _context;

        public DetailsModel(Warehouse.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Item Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Item = await _context.Items
                .Include(i => i.Category).FirstOrDefaultAsync(m => m.ItemID == id);

            if (Item == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
