using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService.Abstractions
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
