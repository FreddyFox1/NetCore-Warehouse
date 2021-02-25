using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Warehouse.Areas.Identity.Data
{
    public class ItemLogs
    {
        public int ID { get; set; }

        [Display(Name = "Название")]
        public string LogItemName { get; set; }

        [Display(Name = "Пользователь")]
        public string LogUserName { get; set; }

        [Display(Name = "Откуда переместил")]
        public string LogOldStorage { get; set; }

        [Display(Name = "Куда переместил")]
        public string LogCurStorage { get; set; }

        [Display(Name = "Артикул")]
        public string LogItemArticle { get; set; }

        [Display(Name = "Дата перемещения")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LogDateTransfer { get; set; }
    }
}

