using System.Threading.Tasks;

namespace Warehouse.Services.TelegramService.TelegramAbstraction
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
