using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Warehouse.Model;

namespace Warehouse.Pages.Items.Groups
{
    [Authorize(Policy = "UserArea")]
    public class CreateModel : PageModel
    {
        private readonly Warehouse.Model.ApplicationDbContext _context;

        public CreateModel(Warehouse.Model.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ItemCategory ItemCategory { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ItemsCategories.Add(ItemCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
