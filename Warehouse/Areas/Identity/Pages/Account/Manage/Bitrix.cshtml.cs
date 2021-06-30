using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Areas.Identity.Pages.Account.Manage
{
    public class BitrixModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BitrixModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<BitrixUser> BitrixUser { get;set; }

        public async Task OnGetAsync()
        {
            BitrixUser = await _context.BitrixUsers.ToListAsync();
        }
    }
}
