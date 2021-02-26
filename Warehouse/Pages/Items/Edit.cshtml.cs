using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse.Areas.Identity.Data;

namespace Warehouse.Pages.Items
{
    public class EditModel : PageModel
    {
        private readonly Warehouse.Areas.Identity.Data.ApplicationDbContext _context;

        public EditModel(Warehouse.Areas.Identity.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["CategoryID"] = new SelectList(_context.ItemsCategories, "CategoryID", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(Item.ItemID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemID == id);
        }
    }
}
