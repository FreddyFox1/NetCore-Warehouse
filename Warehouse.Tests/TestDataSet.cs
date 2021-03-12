using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Model;

namespace Warehouse.Tests
{
    public class TestDataSet
    {

        public async void CreateUser(UserManager<WarehouseUser> _userManager,
                                        SignInManager<WarehouseUser> _signInManager,
                                        ApplicationDbContext _context,
                                        string _email,
                                        string _password)
        {
            var user = new WarehouseUser { UserName = _email, Email = _email };
            var result = await _userManager.CreateAsync(user, _password);
            var signin = await _signInManager.PasswordSignInAsync(_email, _password, true, lockoutOnFailure: false);

        }


    }
}
