using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Areas.Identity.Pages.Account.Manage.Bitrix
{
    [Authorize(Policy = "AdminArea")]
    public class DeleteModel : PageModel
    {
        private readonly Warehouse.Model.ApplicationDbContext _context;

        public DeleteModel(Warehouse.Model.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BitrixUser = await _context.BitrixUsers.FindAsync(id);

            if (BitrixUser != null)
            {
                _context.BitrixUsers.Remove(BitrixUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
