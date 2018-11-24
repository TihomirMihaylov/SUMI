using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SUMI.Models;

namespace SUMI.Data
{
    public class SumiDbContext : IdentityDbContext<InsuranceUser>
    {
        public SumiDbContext(DbContextOptions<SumiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Damage> Damages { get; set; }

        public DbSet<Policy> Policies { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<SumiDbContext>
        {
            public SumiDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<SumiDbContext>();
                optionsBuilder.UseSqlServer<SumiDbContext>("Server=(localdb)\\mssqllocaldb;Database=SUMI;Trusted_Connection=True;MultipleActiveResultSets=true");

                return new SumiDbContext(optionsBuilder.Options);
            }
        }
    }
}