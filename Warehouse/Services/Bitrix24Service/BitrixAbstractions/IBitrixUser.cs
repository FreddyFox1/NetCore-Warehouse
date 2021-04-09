using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Services.Bitrix24Service.BitrixAbstractions
{
    /// <summary>
    /// Интерфейс для работы с сущеностями пользователей Bitrix24
    /// </summary>
    interface IBitrixUser
    {
        /// <summary>
        /// Получаем список пользователей с их данными из Bitrix24
        /// </summary>
        /// <returns>Возвращает список пользователей</returns>
        List<string> GetUsers();
        
        /// <summary>
        /// Проверяем существует ли пользователь в БД
        /// </summary>
        /// <returns>Если пользователь существует, то вернет True</returns>
        bool isUserCreated();
        
        /// <summary>
        /// Сохраняем пользователя в базе данных
        /// </summary>
        void SaveUser();

       
    }
}
