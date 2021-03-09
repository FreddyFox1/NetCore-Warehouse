using Warehouse.Model;

namespace Warehouse.Services.Bitrix24Service.BitrixAbstractions
{
    interface IBitrix
    {
        void CreateTask(Item item);
        void PushTask();
    }
}
