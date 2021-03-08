using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Templating;
using Warehouse.Model;

namespace Warehouse.Pages.Items
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        public List<SelectListItem> GroupsList { get; set; }
        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            GroupsList = _context.ItemsCategories.Select(a =>
                       new SelectListItem
                       {
                           Value = a.CategoryName.ToString(),
                           Text = a.CategoryName + " (" + _context.Items.Where(b => b.CategoryID == a.CategoryID).Count() + ')'
                       }).ToList();
            
            //var Count = _context.Items.Where(a => a.CategoryID == null).Count();
            //GroupsList.Add(new SelectListItem
            //{
            //    Value = "NaN",
            //    Text = String.Format("Без категории ({0})", Count)
            //});

        }

    }
}
