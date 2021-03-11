using Warehouse.Model;

namespace Warehouse.Services.Bitrix24Service.BitrixAbstractions
{
    interface IBitrix
    {
        string CreateTask(Item item);
        bool PushTask(string _fields, string Article);
    }
}
