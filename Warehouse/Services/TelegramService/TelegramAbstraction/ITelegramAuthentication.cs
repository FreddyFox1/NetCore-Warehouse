using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService.TelegramAbstraction
{
    /// <summary>
    /// Telegram бот должен общаться только с авторизированными клиентами
    /// </summary>
    interface ITelegramAuthentication
    {
        bool CheckLogin(string _Email);
        bool CheckAuthorization(string _Email, string _Password);
    }
}
