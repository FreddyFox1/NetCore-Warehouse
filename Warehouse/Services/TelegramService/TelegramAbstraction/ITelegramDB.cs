using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService.TelegramAbstraction
{
    interface ITelegramDB
    {
        void SaveUser();
        void DeleteUsers();
        List<TelegramEntity> GetTelegramEntities();
    }
}
