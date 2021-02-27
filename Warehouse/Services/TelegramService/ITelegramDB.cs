using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService
{
    interface ITelegramDB
    {
        void SaveUser();
        List<TelegramEntity> GetTelegramEntities();
    }
}
