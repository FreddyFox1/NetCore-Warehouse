using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Warehouse.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Warehouse.Areas.Identity.Data;

namespace Warehouse.Pages.Items
{
    [Authorize(Policy = "DriverArea")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<WarehouseUser> _userManager;
        private IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        #region VariableForItems
        [BindProperty]
        public string StorageValue { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
        [BindProperty]
        public List<SelectListItem> ItemGroup { get; set; }
        [BindProperty]
        public int SelectedCategory { get; set; }
        [BindProperty]
        public Item Items { get; set; }
        [BindProperty]
        public string Creator { get; set; }
        #endregion

        public CreateModel(ApplicationDbContext context, UserManager<WarehouseUser> userManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            CreateSelectList();
            IdentityUser UserName = await _userManager.GetUserAsync(User);
            if (UserName == null)
            {
                Creator = "Unknown user";
            }
            else Creator = User.Identity.Name;
            return Page();
        }

        public void CreateSelectList()
        {
            ItemGroup =  _context.ItemsCategories.Select(a =>
                       new SelectListItem
                       {
                           Value = a.CategoryName.ToString(),
                           Text = a.CategoryName
                       }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!String.IsNullOrEmpty(StorageValue))
                if (ValidArticle(Items.ItemArticle))
                {
                    Items.DateTransferItem = DateTime.Now;

                    if (Image != null)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                        var pathFile = Path.Combine(_environment.WebRootPath, "img");
                        var file = Path.Combine(pathFile, uniqueFileName);
                        //Save on server file
                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await Image.CopyToAsync(fileStream);
                        }
                        //Compress image
                        ImageResizer.Resizer(pathFile, uniqueFileName, file);
                        //Save FileName in DB
                        Items.ItemPhoto = uniqueFileName;
                    }
                    else
                    {
                        Items.ItemPhoto = "noimage.jpg";
                    }

                    Items.CategoryID = SelectedCategory;
                    Items.Creator = Creator;
                    Items.ItemStorageID = StorageValue;

                    _context.Items.Add(Items);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError("", "Указанный артикул уже существует");
                    CreateSelectList();
                    return Page();
                }
            else
            {
                ModelState.AddModelError("", "Склад не указан");
                CreateSelectList();
                return Page();
            }
        }


        public bool ValidArticle(string Article)
        {
            var temp = _context.Items.FirstOrDefault(a => a.ItemArticle == Article);
            if (temp == null)
            {
                return true;
            }
            else return false;
        }
    }
}
