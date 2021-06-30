using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Services.TelegramService;

namespace Warehouse.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = "AdminArea")]
    public class TelegramModel : PageModel
    {
        private readonly Warehouse.Model.ApplicationDbContext _context;

        public TelegramModel(Warehouse.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TelegramEntity> TelegramEntities { get; set; }

        public async Task OnGetAsync()
        {
            TelegramEntities = await _context.TelegramEntities.ToListAsync();
        }
    }
}
