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
using Warehouse.Model;
using Warehouse.Services.Bitrix24Service;
using Warehouse.Services.Bitrix24Service.BitrixAbstractions;


namespace Warehouse.Pages.Items
{
    [Authorize(Policy = "DriverArea")]
    public class CreateModel : PageModel
    {
        private readonly IBitrix _bitrix;
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

        public CreateModel(ApplicationDbContext context, UserManager<WarehouseUser> userManager, IWebHostEnvironment environment, IBitrix bitrix)
        {
            _userManager = userManager;
            _context = context;
            _bitrix = bitrix;
            _environment = environment;
        }

        /// <summary>
        /// Страница создания новой записи
        /// </summary>
        /// <returns>Возвращает страницу создания записи в БД</returns>
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
        
        /// <summary>
        /// Получаем список категорий из БД
        /// </summary>
        public void CreateSelectList()
        {
            ItemGroup = _context.ItemsCategories.Select(a =>
                      new SelectListItem
                      {
                          Value = a.CategoryID.ToString(),
                          Text = a.CategoryName
                      }).ToList();
        }

        /// <summary>
        /// Добавление новой позиции в БД
        /// </summary>
        /// <returns>
        ///     В случаем успешного добавление возвращает :Items/Index:"
        /// </returns>
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
                    //_bitrix.SendNotyfication(, $"Добавлена новая мастер модель: {Items.ItemName}");
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
        
        /// <summary>
        /// Проврека на уникальность артикула
        /// </summary>
        /// <param name="Article"></param>
        /// <returns>Если артикул уникален возвращает true</returns>
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
