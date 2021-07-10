using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Model
{
    /// <summary>
    /// Модель пользователей из Bitrix24
    /// </summary>
    public class BitrixUser
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Id пользователя")]
        public string UserId { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "ВКЛ/ВЫКЛ")]
        public bool isSigned { get; set; }
    }
}
