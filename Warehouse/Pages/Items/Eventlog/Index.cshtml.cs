using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Warehouse.Model;

namespace Warehouse.Pages.Items.Eventlog
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context,ILogger<IndexModel> logger )
        {
            _logger = logger;
            _context = context;
        }

        public IList<ItemLog> LogsItems { get; set; }
        public async Task OnGetAsync()
        {
            LogsItems = await _context.ItemLogs.ToListAsync();
        }
    }
}
