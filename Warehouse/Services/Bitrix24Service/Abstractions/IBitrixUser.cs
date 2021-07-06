using Warehouse.Model;

namespace Warehouse.Services.Bitrix24Service.Abstractions
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
        void GetUsers();

        /// <summary>
        /// Проверяем существует ли пользователь в БД
        /// </summary>
        /// <returns>Если пользователь существует, то вернет True</returns>
        BitrixUser isUserExist(string userId);

    }
}
