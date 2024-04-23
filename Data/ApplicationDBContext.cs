using Data.Commons;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opt) : base(opt) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(_ => !_.IsDeleted);
            modelBuilder.Entity<Booking>().HasQueryFilter(_ => !_.IsDeleted);

            modelBuilder.Entity<User>().HasData(
               new User { Id = 1, Name = "Owner", Email = "sa@mailinator.com", Role = enRole.Admin, Password = "$2a$11$LF.jO5445FGwpoGW9PGgR.TKNymOmleYKS2vPhTcpqanjMM9stbIC" });
        }
    }
}
