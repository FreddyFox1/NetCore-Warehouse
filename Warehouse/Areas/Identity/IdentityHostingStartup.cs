﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Areas.Identity.Data;

[assembly: HostingStartup(typeof(Warehouse.Areas.Identity.IdentityHostingStartup))]
namespace Warehouse.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ApplicationDbContextConnection")));

                services.AddDefaultIdentity<WarehouseUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            });
        }
    }
}