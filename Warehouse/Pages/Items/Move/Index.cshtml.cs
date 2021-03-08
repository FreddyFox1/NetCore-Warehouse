using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Warehouse.Model;

namespace Warehouse.Pages.Items.Move
{
    [Authorize(Policy = "DriverArea")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public string NewStorageValue { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Item> Item { get;set; }

        public async Task OnGetAsync()
        {
            Item = await _context.Items.ToListAsync();
        }
    }
}
