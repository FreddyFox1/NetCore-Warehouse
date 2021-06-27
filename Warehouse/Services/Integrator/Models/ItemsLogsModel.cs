using System.Collections.Generic;
using Warehouse.Model;

namespace Warehouse.Services.Integrator.Models
{
    public partial class IntegratorService
    {
        public class ItemsLogsModel
        {
            public List<ItemLog> ItemLog { get; set; }
        }
    }
}
