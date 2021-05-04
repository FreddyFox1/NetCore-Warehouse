using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Warehouse.Controllers;
using Microsoft.AspNetCore.Authorization;
using Warehouse.Model;

namespace Warehouse.Pages.Items
{
    [Authorize(Policy = "DriverArea")]
    public class EditModel : PageModel
    {
        #region Variables 
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        [BindProperty]
        public List<SelectListItem> ItemGroup { get; set; }
        [BindProperty]
        public int SelectedCategory { get; set; }
        [BindProperty]
        public IFormFile NewImage { get; set; }
        [BindProperty]
        public string OldFileName { get; set; }
        [BindProperty]
        public Item Items { get; set; }
        [BindProperty]
        public string CurrentIteamStock { get; set; }
        #endregion

        public EditModel(ApplicationDbContext context, IWebHostEnvironment environment)//IHostingEnvironment environment)
        {
            _environment = environment;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                if (User.IsInRole("Driver"))
                {
                    return Redirect($"DriveEdit?id={id}");
                }
                else
                {
                    Items = await _context.Items.FirstOrDefaultAsync(m => m.ItemID == id);
                    CreateSelectList();
                    SelectedCategory = Items.CategoryID;

                    if (Items == null)
                    {
                        return NotFound($"Unable to load user with ID.");
                    }
                    else
                    {
                        OldFileName = Items.ItemPhoto;
                    }

                    return Page();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(Items).State = EntityState.Modified;
            try
            {
                Items.CategoryID = SelectedCategory;
                //Загрузка изображения и удаление старого
                if (NewImage != null)
                {
                    Items.ItemPhoto = await SaveNewImage();
                    if (OldFileName != ("noimage.jpg"))
                        DeleteOldImage(OldFileName);
                }
                else
                {
                    Items.ItemPhoto = OldFileName;
                }
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(Items.ItemID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemID == id);
        }

        private void DeleteOldImage(string _path)
        {
            string filePath_Icon = Path.Combine(_environment.WebRootPath, "img", "Icons", _path);
            System.IO.File.Delete(filePath_Icon);
            string filePath_Photo = Path.Combine(_environment.WebRootPath, "img", "Photo", _path);
            System.IO.File.Delete(filePath_Photo);
        }

        private async Task<string> SaveNewImage()
        {
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + NewImage.FileName;
            var pathFile = Path.Combine(_environment.WebRootPath, "img");
            var file = Path.Combine(pathFile, uniqueFileName);
            //Save on server new file
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await NewImage.CopyToAsync(fileStream);
            }
            //Compress image
            ImageResizer.Resizer(pathFile, uniqueFileName, file);
            return uniqueFileName;
        }

        public void CreateSelectList()
        {
            ItemGroup = _context.ItemsCategories.Select(a =>
                       new SelectListItem
                       {
                           Value = a.CategoryName.ToString(),
                           Text = a.CategoryName
                       }).ToList();
        }
    }
}
