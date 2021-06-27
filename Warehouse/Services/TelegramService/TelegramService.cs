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
using Warehouse.Services.TelegramService.Abstractions;
using Warehouse.Services.TelegramService.Commands;
using Warehouse.Model;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Warehouse.Services.TelegramService
{
    internal class TelegramService : IHostedService, ITelegram, ITelegramDB
    {
        private static ILogger<TelegramService> logger;
        private Timer timer;
        private static TelegramBotClient telegramClient;
        private readonly IOptions<TelegramKey> telegramKey;
        private static List<CommandBase> CommandsList;
        private readonly ApplicationDbContext _db;

        public TelegramService(ILogger<TelegramService> _logger,
                               IOptions<TelegramKey> _telegramKey,
                               ApplicationDbContext db)
        {
            logger = _logger;
            telegramKey = _telegramKey;
            telegramClient = new TelegramBotClient(telegramKey.Value.AuthKey);
            _db = db;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            CreateCommandList();

            logger.LogInformation("Telegram service started");
            telegramClient.OnUpdate += OnUpdateReceived;
            
            timer = new Timer(a =>
            {
                GetUpdates();
            }, 
            null, TimeSpan.Zero, TimeSpan.FromSeconds(8));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Telegram service was stoped");
            return Task.CompletedTask;
        }

        public void GetUpdates()
        {
            telegramClient.StartReceiving(Array.Empty<UpdateType>());
        }

        private async void OnUpdateReceived(object sender, UpdateEventArgs e)
        {
            var message = e.Update.Message;
            if (message != null)
            {
                if (message.Type == MessageType.Text)
                {
                    foreach (var command in CommandsList)
                    {
                        if (command.Contains(message.Text))
                        {
                            command.Execute(message, telegramClient);
                            break;
                        }
                    }
                }

                if (message.Type == MessageType.Contact)
                {
                    foreach (var command in CommandsList)
                    {
                        if (command.Contains("Contact"))
                        {
                            command.Execute(message, telegramClient);

                            TelegramEntity user = new TelegramEntity()
                            {
                                ID = message.Contact.UserId,
                                ChatID = message.Chat.Id,
                                FirstName = message.Contact.FirstName,
                                LastName = message.Contact.LastName,
                                PhoneNumber = message.Contact.PhoneNumber
                            };

                            SaveUser(user);
                            break;
                        }
                    }
                }
                else return;
            }
        }

        private void CreateCommandList()
        {
            CommandsList = new List<CommandBase>();
            CommandsList.Add(new StartCommand());
            CommandsList.Add(new SearchCommand());
            CommandsList.Add(new ContactCommand());
        }

        public void SaveUser(TelegramEntity entity)
        {
            bool isExist = GetEntitiyByPhoneNumber(entity.PhoneNumber);
            if (!isExist)
            {
                _db.Add(entity);
                _db.SaveChangesAsync();
                logger.LogInformation($"Добавлен новый пользователь с номером {entity.PhoneNumber}");
            }
        }

        public bool GetEntitiyByPhoneNumber(string PhoneNumber)
        {
            bool isExist = _db.TelegramEntities.Any(x => x.PhoneNumber == PhoneNumber);

            if (isExist == true) logger.LogInformation($"Пользователь с номером {PhoneNumber} уже существует");
            else logger.LogInformation($"Пользователь с номером {PhoneNumber} не существует");

            return isExist;
        }

        public void DeleteUser()
        {
            throw new NotImplementedException();
        }
        public void SendNotification()
        {
            throw new NotImplementedException();
        }
    }
}
