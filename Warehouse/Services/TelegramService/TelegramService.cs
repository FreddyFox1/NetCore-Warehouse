using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Args;
using Microsoft.Extensions.Options;
using Warehouse.Services.TelegramService.TelegramAbstraction;

namespace Warehouse.Services.TelegramService
{
    internal class TelegramService : IHostedService, ITelegram
    {
        private static ILogger<TelegramService> _logger;
        private Timer timer;
        private static TelegramBotClient telegramClient;
        private readonly IOptions<TelegramKey> telegramKey;
        private static List<Command> CommandsList;
        public static IReadOnlyList<Command> Commands
        {
            get => CommandsList.AsReadOnly();
        }
        /// <summary>
        /// </summary>
        /// <param name="logger">Получаем логгер для логгирования</param>
        /// <param name="_telegramKey">Token атвторизации для работы бота Telegram</param>
        public TelegramService(ILogger<TelegramService> logger, IOptions<TelegramKey> _telegramKey)
        {
            _logger = logger;
            telegramKey = _telegramKey;
            telegramClient = new TelegramBotClient(telegramKey.Value.AuthKey);
        }

        /// <summary>
        /// Подписываемся на событие обновления сообщений, 
        /// и начинаем прослушивать сообщения которые летят к боту
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            SetCommandList();
            
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

        /// <summary>
        /// Получение обновлений от Telegram API
        /// </summary>
        public void GetUpdates()
        {
            telegramClient.StartReceiving(Array.Empty<UpdateType>());
        }

        /// <summary>
        /// Отправка уведомлений
        /// </summary>
        public void SendNotification()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Событие получения обновлений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static async void OnUpdateReceived(object sender, UpdateEventArgs e)
        {
            var message = e.Update.Message;
            if (message == null || message.Type != MessageType.Text) return;
            else
            {
                foreach (var command in Commands)
                {
                    if (command.Contains(message.Text))
                    {
                        command.Execute(message, telegramClient);
                        break;
                    }
                }
                _logger.LogWarning(message.Text);
            };
        }

        /// <summary>
        /// Создаем и заполняем список команд бота
        /// </summary>
        private void SetCommandList()
        {
            CommandsList = new List<Command>();
            CommandsList.Add(new StartCommand());
        }
    }

}
