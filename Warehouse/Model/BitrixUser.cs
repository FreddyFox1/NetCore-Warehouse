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
        [Display(Name="ID пользователя")]
        public int UserID { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "ВКЛ/ВЫКЛ")]
        public bool isSigned { get; set; }
    }
}
