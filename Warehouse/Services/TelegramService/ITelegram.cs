using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService
{
    /// <summary>
    /// 
    /// </summary>
    interface ITelegram
    {
        void GetUpdates();
        void SendNotification();
    }
}
