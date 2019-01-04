namespace SUMI.Services.Data.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SUMI.Data;
    using SUMI.Data.Models;
    using SUMI.Data.Repositories;
    using SUMI.Services.Data.Damages;
    using Xunit;

    public class DamageServiceTests
    {
        [Fact]
        public async Task AddDamageShouldIncreaseCountOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb1")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            var newDamage = new Damage();
            Assert.Equal(0, repository.All().Count());
            await service.AddDamage(newDamage);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task AddDamageShouldAddTheCorrectObject()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb2")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            var newDamage = new Damage { Id = 1, PartName = "Door" };
            await service.AddDamage(newDamage);
            var damageFromDb = await repository.GetByIdAsync(newDamage.Id);
            Assert.Equal<Damage>(newDamage, damageFromDb);
        }

        [Fact]
        public async Task RemoveDamageShouldDoNothingOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb3")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            int randomId = 1;
            var result = await service.RemoveDamage(randomId);
            Assert.Equal(-1, result);
            Assert.Equal(0, repository.All().Count());
        }

        [Fact]
        public async Task RemoveDamageShouldDoNothingWithNonExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb4")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Damages.Add(new Damage { Id = 1 });
            dbContext.Damages.Add(new Damage { Id = 2 });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            int nonExistingId = 3;
            await service.RemoveDamage(nonExistingId);
            Assert.Equal(2, repository.All().Count());
        }

        [Fact]
        public async Task RemoveDamageShouldReduceCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb5")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Damages.Add(new Damage { Id = 1 });
            dbContext.Damages.Add(new Damage { Id = 2 });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            int existingId = 2;
            await service.RemoveDamage(existingId);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task SettleDamageShouldDoNothingOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb6")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            int randomId = 1;
            await service.SettleDamage(randomId, 200);
            Assert.Equal(0, repository.All().Count());
        }

        [Fact]
        public async Task SettleDamageShouldDoNothingWithNonExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb7")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var damage1 = new Damage { Id = 1, EstimatedCost = 100, IsSettled = true };
            var damage2 = new Damage { Id = 2, EstimatedCost = 0, IsSettled = false };
            dbContext.Damages.Add(damage1);
            dbContext.Damages.Add(damage2);
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            int nonExistingId = 3;
            await service.SettleDamage(nonExistingId, 200);
            Assert.Equal(2, repository.All().Count());
            var damage1FromDb = await repository.GetByIdAsync(damage1.Id);
            var damage2FromDb = await repository.GetByIdAsync(damage2.Id);
            Assert.Equal<Damage>(damage1, damage1FromDb);
            Assert.Equal<Damage>(damage2, damage2FromDb);
        }

        [Fact]
        public async Task SettleDamageShouldModifyEntityWithExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb8")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var damage = new Damage { Id = 1, EstimatedCost = 0, IsSettled = false };
            dbContext.Damages.Add(damage);
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            int existingId = 1;
            decimal estimatedCost = 200;
            await service.SettleDamage(existingId, estimatedCost);
            var damageFromDb = await repository.GetByIdAsync(damage.Id);
            Assert.Equal(estimatedCost, damageFromDb.EstimatedCost);
            Assert.True(damageFromDb.IsSettled);
        }

        [Fact]
        public async Task GetTotalAmountSpentForPolicyShouldReturnZeroWithNonExistingPolicyId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb9")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var damage1 = new Damage { Id = 1, PolicyId = "myTestPolicy", EstimatedCost = 100 };
            var damage2 = new Damage { Id = 2, PolicyId = "myTestPolicy", EstimatedCost = 200 };
            dbContext.Damages.Add(damage1);
            dbContext.Damages.Add(damage2);
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            string nonExistingPolicyId = "ABC";
            var result = service.GetTotalAmountSpentForPolicy(nonExistingPolicyId);
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetTotalAmountSpentForPolicyShouldReturnCorrectResultWithExistingPolicyId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb10")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var damage1 = new Damage { Id = 1, PolicyId = "myTestPolicy", EstimatedCost = 100 };
            var damage2 = new Damage { Id = 2, PolicyId = "myTestPolicy", EstimatedCost = 200 };
            dbContext.Damages.Add(damage1);
            dbContext.Damages.Add(damage2);
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Damage>(dbContext);
            var service = new DamageService(repository);
            string existingPolicyId = "myTestPolicy";
            var result = service.GetTotalAmountSpentForPolicy(existingPolicyId);
            Assert.Equal(300, result);
        }
    }
}
