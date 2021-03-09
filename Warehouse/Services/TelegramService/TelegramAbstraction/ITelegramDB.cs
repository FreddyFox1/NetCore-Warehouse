﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService.TelegramAbstraction
{
    /// <summary>
    /// Интерфейс для работы с базой данных
    /// </summary>
    interface ITelegramDB
    {
        void SaveUser();
        void DeleteUsers();
        List<TelegramEntity> GetTelegramEntities();
    }
}
