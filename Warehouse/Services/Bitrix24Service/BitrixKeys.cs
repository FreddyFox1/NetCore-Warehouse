using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Services.Bitrix24Service
{
    /// <summary>
    /// Ключ авторизации и адрес портала Bitrix24 для работы с API 
    /// </summary>
    public class BitrixKeys
    {
        public string AuthKey { get; set; }
        public string URL { get; set; }
    }
}
