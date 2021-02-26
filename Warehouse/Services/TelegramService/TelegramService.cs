using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Warehouse.Services.TelegramService
{
    public class TelegramService : ITelegramService
    {
        private static TelegramBotClient telegram;
        private static int offset;
       
        /// <summary>
        /// Создаем объект для работы с чат ботом в телеграм
        /// </summary>
        /// <param name="token">Токен авторизации для работы бота</param>
        /// <param name="_offset">Смещение </param>
        public TelegramService(string token, int _offset)
        {
            telegram = new TelegramBotClient(token);
            offset = _offset;
        }

        /// <summary>
        /// Получаем сообщения из телеграм чата, ждем клиентов которые подпишутся на обновления
        /// </summary>
        public async Task getUpdates()
        {
            try
            {
                var updates = await telegram.GetUpdatesAsync(offset, 0, 5);

                foreach (var update in updates)
                {
                    if (update.Message != null)
                    {
                        if (update.Message.Type == MessageType.Contact)
                        {
                        }
                        else
                        {
                        }

                    }
                    offset = update.Id + 1;
                }
                await Task.Delay(5000);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
