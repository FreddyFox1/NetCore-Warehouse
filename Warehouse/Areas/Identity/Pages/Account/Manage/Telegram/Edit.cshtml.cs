using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;
using Warehouse.Services.TelegramService;

namespace Warehouse.Areas.Identity.Pages.Account.Manage.Telegram
{
    public class EditModel : PageModel
    {
        private readonly Warehouse.Model.ApplicationDbContext _context;

        public EditModel(Warehouse.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TelegramEntity TelegramEntity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TelegramEntity = await _context.TelegramEntities.FirstOrDefaultAsync(m => m.ID == id);

            if (TelegramEntity == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TelegramEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelegramEntityExists(TelegramEntity.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Telegram");
        }

        private bool TelegramEntityExists(int id)
        {
            return _context.TelegramEntities.Any(e => e.ID == id);
        }
    }
}
