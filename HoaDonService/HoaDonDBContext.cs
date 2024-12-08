using HoaDonService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace HoaDonService
{
    public class HoaDonDBContext : DbContext
    {
        public HoaDonDBContext(DbContextOptions<HoaDonDBContext> options) : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DbSet<HoaDon> HoaDons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some initial data
            modelBuilder.Entity<HoaDon>().HasData(
                new HoaDon { Id = 1, Title = "Thanh toan bao hiem Y te", MSSV = 1001, Money = 3000000, Status = Status.CHUA_THANH_TOAN },
                new HoaDon { Id = 2, Title = "Thanh toan tien dien thoai", MSSV = 1001, Money = 5000000, Status = Status.CHUA_THANH_TOAN },
                new HoaDon { Id = 3, Title = "Thanh toan tien thuoc", MSSV = 1001, Money = 9000000, Status = Status.CHUA_THANH_TOAN },
                new HoaDon { Id = 4, Title = "Thanh toan don dat hang", MSSV = 1002, Money = 5000000, Status = Status.CHUA_THANH_TOAN },
                new HoaDon { Id = 5, Title = "Thanh toan don dat hang", MSSV = 1002, Money = 10000000, Status = Status.CHUA_THANH_TOAN }
            );
        }
    }
}
