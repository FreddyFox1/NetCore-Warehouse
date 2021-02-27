using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Configuration;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Args;

namespace Warehouse.Services.TelegramService
{
    internal class TelegramService : IHostedService, ITelegram
    {
        private static ILogger<TelegramService> _logger;
        private Timer timer;
        private TelegramBotClient telegramClient = new TelegramBotClient("1679002332:AAFmai6ZuuAnQ5MAlhOtjI6HmqFlAMSE--o");

        public TelegramService(ILogger<TelegramService> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Telegram service started");
            telegramClient.OnUpdate += OnUpdateReceived;
            timer = new Timer(a =>
            {
                GetUpdates();
            },
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Telegram service was stoped");
            return Task.CompletedTask;
        }

        public void GetUpdates()
        {
            telegramClient.StartReceiving(Array.Empty<UpdateType>());
        }

        public void SendNotification()
        {
            throw new NotImplementedException();
        }

        private static async void OnUpdateReceived(object sender, UpdateEventArgs e)
        {
            var message = e.Update.Message;
            if (message == null || message.Type != MessageType.Text) return;
            _logger.LogWarning(message.Text);
        }

    }

}
