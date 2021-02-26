﻿using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService
{
    public class TelegramEntity
    {
        [Key]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "ID Чата")]
        public int ChatID { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Доступ к уведомлениям")]
        public bool _sendUpdates { get; set; } = false;
    }
}
