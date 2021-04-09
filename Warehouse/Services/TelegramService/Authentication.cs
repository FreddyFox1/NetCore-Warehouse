using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Model;
using Warehouse.Services.TelegramService.TelegramAbstraction;

namespace Warehouse.Services.TelegramService
{
    public class Authentication : ITelegramAuthentication
    {
        private readonly static UserManager<WarehouseUser> userManager;
        private readonly static SignInManager<WarehouseUser> signInManager;

        /// <summary>
        /// Проверка логина пользователя
        /// </summary>
        /// <param name="_Email">Логин пользователя от приложения</param>
        /// <returns>Возвращает true если пользователь существует</returns>
        public bool CheckLogin(string _Email)
        {
            var user = userManager.FindByEmailAsync(_Email);
            if (user != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="_Email">Логин пользователя от приложения</param>
        /// <param name="_Password">Пароль пользователя от приложения</param>
        /// <returns>Возвращает true если пользователь успешно прошел авторизацию</returns>
        public bool CheckAuthorization(string _Email, string _Password)
        {
            throw new NotImplementedException();
        }
    }
}
