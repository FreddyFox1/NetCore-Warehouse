using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Areas.Identity.Pages.Account.Manage.Bitrix
{
    public class EditModel : PageModel
    {
        private readonly Warehouse.Model.ApplicationDbContext _context;

        public EditModel(Warehouse.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BitrixUser BitrixUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BitrixUser = await _context.BitrixUsers.FirstOrDefaultAsync(m => m.UserID == id);

            if (BitrixUser == null)
            {
                return NotFound();
            }
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

            _context.Attach(BitrixUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BitrixUserExists(BitrixUser.UserID))
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

        private bool BitrixUserExists(int id)
        {
            return _context.BitrixUsers.Any(e => e.UserID == id);
        }
    }
}
