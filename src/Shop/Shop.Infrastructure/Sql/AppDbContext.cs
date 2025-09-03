using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;
using System.Reflection;

namespace Shop.Infrastructure.Sql
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho trigger trên bảng Users
            modelBuilder.Entity<User>().ToTable(tb => tb.HasTrigger("trg_AfterInsertUser_PopulateUserProductInteractions"));

            // Cấu hình cho trigger trên bảng Phones
            modelBuilder.Entity<Phone>().ToTable(tb => tb.HasTrigger("trg_AfterInsertPhone_PopulateUserProductInteractions"));

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
