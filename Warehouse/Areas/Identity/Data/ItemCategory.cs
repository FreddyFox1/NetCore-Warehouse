using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Areas.Identity.Data
{
    public class ItemCategory
    {
        [Key]
        [Display(Name = "ID")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Выберите категорию")]
        [Display(Name = "Категория")]
        public string CategoryName { get; set; }

        public List<Item> Item { get; set; }
    }
}
