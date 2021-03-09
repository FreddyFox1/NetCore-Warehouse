using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService.TelegramAbstraction
{
    /// <summary>
    /// Интерфейс для работы с Telegram API
    /// </summary>
    interface ITelegram
    {
        void GetUpdates();
        void SendNotification();
    }
}
