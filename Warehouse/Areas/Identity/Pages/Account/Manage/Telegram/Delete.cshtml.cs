using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;
using Warehouse.Services.TelegramService;

namespace Warehouse.Areas.Identity.Pages.Account.Manage.Telegram
{
    public class DeleteModel : PageModel
    {
        private readonly Warehouse.Model.ApplicationDbContext _context;

        public DeleteModel(Warehouse.Model.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TelegramEntity = await _context.TelegramEntities.FindAsync(id);

            if (TelegramEntity != null)
            {
                _context.TelegramEntities.Remove(TelegramEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Telegram");
        }
    }
}
