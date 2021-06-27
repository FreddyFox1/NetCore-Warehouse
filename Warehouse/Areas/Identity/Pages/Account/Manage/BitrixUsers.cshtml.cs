using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Pages.Admin
{
    public class BitrixUsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BitrixUsersModel(ApplicationDbContext context)
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
