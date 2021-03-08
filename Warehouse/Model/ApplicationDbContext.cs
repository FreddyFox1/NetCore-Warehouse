using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Warehouse.Services.TelegramService;

namespace Warehouse.Model
{
    public class ApplicationDbContext : IdentityDbContext<WarehouseUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemLog> ItemLogs { get; set; }
        public DbSet<ItemCategory> ItemsCategories { get; set; }
        public DbSet<TelegramEntity> TelegramEntities { get; set; }
        
    }
}
