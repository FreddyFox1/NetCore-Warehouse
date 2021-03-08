using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Model
{
    public class Item
    {
        [Key]
        [Display(Name = "ID")]
        public int ItemID { get; set; }
        [Display(Name = "Фото")]
        public string ItemPhoto { get; set; } = "noimage.jpg";

        [Required(ErrorMessage = "Не указано название")]
        [Display(Name = "Название:")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Укажите артикул")]
        [Display(Name = "Артикул:")]
        public string ItemArticle { get; set; }

        [Display(Name = "Текущий склад:")]
        public string ItemStorageID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата перемещения")]
        public DateTime DateTransferItem { get; set; }

        [Display(Name = "Создатель")]
        public string Creator { get; set; }

        [Display(Name = "Примечание:")]
        public string ItemDescription { get; set; }

        [Display(Name = "Размеры:")]
        public string ItemSizes { get; set; }

        [Display(Name = "Кол-во:")]
        public int ItemCount { get; set; }

        [Display(Name = "Ячейка:")]
        public string ItemCell { get; set; }

        [Display(Name = "Закрытая позиция")]
        public bool ItemProtect { get; set; }

        [Display(Name = "ID категории:")]
        public int CategoryID { get; set; }

        [Display(Name = "Категория:")]
        public ItemCategory Category { get; set; }


    }
}
