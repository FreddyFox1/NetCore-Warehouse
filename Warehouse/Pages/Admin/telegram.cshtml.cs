using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;
using Warehouse.Services.TelegramService;

namespace Warehouse.Pages.admin
{
    public class telegramModel : PageModel
    {
        private readonly Warehouse.Model.ApplicationDbContext _context;

        public telegramModel(Warehouse.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TelegramEntity> TelegramEntities { get;set; }

        public async Task OnGetAsync()
        {
            TelegramEntities = await _context.TelegramEntities.ToListAsync();
        }
    }
}
