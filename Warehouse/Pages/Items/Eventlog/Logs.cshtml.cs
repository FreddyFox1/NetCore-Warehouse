using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Pages.Items.Eventlog
{
    public class LogsModel : PageModel
    {
        private readonly Warehouse.Model.ApplicationDbContext _context;

        public LogsModel(Warehouse.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ItemLog> ItemLog { get;set; }

        public async Task OnGetAsync()
        {
            ItemLog = await _context.ItemLogs.ToListAsync();
        }
    }
}
