using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService.Abstractions
{
    /// <summary>
    /// Интерфейс для работы с базой данных
    /// </summary>
    interface ITelegramDb
    {
        Task SaveUser(TelegramEntity entity);
        void DeleteUser();
    }
}
