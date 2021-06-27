using System.Collections.Generic;
using Warehouse.Model;

namespace Warehouse.Services.Integrator.Models
{
    public partial class IntegratorService
    {
        public class CategoriesModel
        {
            public List<ItemCategory> Categories { get; set; }
        }
    }
}
