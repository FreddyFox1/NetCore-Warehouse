﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Model;
using Warehouse.Services.Bitrix24Service.Abstractions;
using Warehouse.Services.Bitrix24Service.Models;

namespace Warehouse.Services.Bitrix24Service
{
    /// <summary>
    /// Сервис для работы с API Bitrix24
    /// </summary>
    public class BitrixService : IBitrix, IBitrixUser
    {
        private readonly IOptions<BitrixKeys> BitrixKeys;
        private readonly ILogger<BitrixService> logger;
        private readonly ApplicationDbContext _db;
        private readonly static RestClient Client = new RestClient();
        private readonly List<BitrixUser> users;

        public BitrixService(IOptions<BitrixKeys> _BitrixKeys, ILogger<BitrixService> _logger, ApplicationDbContext db)
        {
            BitrixKeys = _BitrixKeys;
            logger = _logger;
            _db = db;
            users = _db.BitrixUsers.Where(x => x.isSigned).ToList();
        }

        public string CreateTask(Item item)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"fields[TITLE]= Новая позиция/форма для вывода на сайты: {(!String.IsNullOrEmpty(item.ItemName) ? item.ItemName : "")}");
                builder.Append("&fields[RESPONSIBLE_ID]=546");
                builder.Append("&fields[DESCRIPTION]=");
                builder.Append($"Артикул: {(!String.IsNullOrEmpty(item.ItemArticle) ? item.ItemArticle : "")}\n");
                builder.Append($"Дата запуска в работу: {(!String.IsNullOrEmpty(item.DateTransferItem.ToString()) ? item.DateTransferItem : "")}\n");
                builder.Append($"Открытая / закрытая позиция: \n");
                builder.Append($"Название: {(!String.IsNullOrEmpty(item.ItemName) ? item.ItemName : "")}\n");
                builder.Append($"Размер: {(!String.IsNullOrEmpty(item.ItemSizes) ? item.ItemSizes : "")}\n");
                builder.Append("Где формуется: \n");
                builder.Append("Ед.измерения для формы: \n");
                builder.Append("Цена на форму(2, 3, 4 мм):\n");
                builder.Append("Отлить бетонный экземпляр:\n");
                builder.Append("Ед.измерения для бетона:\n");
                builder.Append("Цена на бетонный изделие:\n");
                builder.Append("Назначение:\n");
                builder.Append($"Раздел на сайте(категория):{(!String.IsNullOrEmpty(item.Category.CategoryName) ? item.Category.CategoryName : "")}\n");
                return builder.ToString();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return "";
            }
        }

        public bool PushTask(string _fields, string Article)
        {
            var request = new RestRequest(BitrixKeys.Value.URL
                + BitrixKeys.Value.AuthKey
                + "/tasks.task.add.json?" + _fields);
            var Response = Client.Post(request);
            logger.LogInformation("Новая задача добавлена в Bitrix24");
            return Response.Content.Contains(Article) ? true : false;
        }

        public async void SendNotyfication(string message)
        {
            foreach (var user in users)
            {
                var request = new RestRequest(BitrixKeys.Value.URL + BitrixKeys.Value.AuthKey + $"/im.notify.json?message={message}&to={user.UserId}");
                await Client.ExecutePostAsync(request);
                await Task.Delay(5000);
            }

            logger.LogInformation($"Уведомление с текстом [{message}] отправлено пользователям");
        }

        public async Task GetUsersAsync()
        {
            var request = new RestRequest(BitrixKeys.Value.URL + BitrixKeys.Value.AuthKey + $"/user.get.json");
            var response = Client.Execute(request);
            var data = JsonConvert.DeserializeObject<BitrixJsonData.Root>(response.Content);

            foreach (var item in data.result)
            {
                BitrixUser user = _db.BitrixUsers.FirstOrDefault(x => x.UserId == item.ID);

                if (user != null)
                {
                    user.UserId = item.ID;
                    user.Name = item.NAME;
                    user.Surname = item.SECOND_NAME;
                    user.Email = item.EMAIL;
                    _db.BitrixUsers.Update(user);
                }

                else
                {
                    var newUser = new BitrixUser
                    {
                        UserId = item.ID,
                        Name = item.NAME,
                        Surname = item.SECOND_NAME,
                        Email = item.EMAIL,
                        isSigned = false
                    };
                    await _db.AddAsync(newUser);
                }
            }

            await _db.SaveChangesAsync();
            logger.LogInformation("Список пользователей обновлен");
        }
    }
}
