using System.Collections.Generic;
using Warehouse.Model;

namespace Warehouse.Services.Integrator.Models
{
    public partial class IntegratorService
    {
        public class ItemsModel
        {
            public List<Item> Items { get; set; }
        }
    }
}
