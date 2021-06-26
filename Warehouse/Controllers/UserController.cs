using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Warehouse.Model;

namespace Warehouse.Controllers
{
    [Route("api/users")]
    [ApiController]
    //[Authorize(Roles = "AdminArea")]
    public class UserController: Controller
    {
        private readonly UserManager<WarehouseUser> _userManager;
        public UserController(UserManager<WarehouseUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> Delete(string Id)
        {
            WarehouseUser user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Успешное удаление" });
                }
            }
            return Json(new { success = true, message = "Ошибка удаление" });

        }
    }
}
