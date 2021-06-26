using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Warehouse.Model
{
    public class WarehouseUser : IdentityUser
    {
        public string PhotoPath { get; set; }
        public string Name { get; set; }
   
    }   
}
