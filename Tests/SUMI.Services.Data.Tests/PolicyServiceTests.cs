namespace SUMI.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SUMI.Data;
    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;
    using SUMI.Data.Repositories;
    using SUMI.Services.Data.Policies;
    using Xunit;

    public class PolicyServiceTests
    {
        [Fact]
        public void IsVehicleInsuredShouldReturnFalseOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>().AsQueryable());
            var service = new PolicyService(repository.Object);
            int nonExistingVehicleId = 1;
            Assert.False(service.IsVehicleInsured(nonExistingVehicleId));
        }

        [Fact]
        public void IsVehicleInsuredShouldReturnFalseOnNonExistingVehicle()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var vehicle = new Vehicle { Id = 1 };
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>
                                                    {
                                                        new Policy { VehicleId = vehicle.Id, IsValid = true },
                                                    }.AsQueryable());
            var service = new PolicyService(repository.Object);
            int nonExistingVehicleId = 2;
            Assert.False(service.IsVehicleInsured(nonExistingVehicleId));
        }

        [Fact]
        public void IsVehicleInsuredShouldReturnFalseOnExistingNotInsuredVehicle()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var vehicle = new Vehicle { Id = 1 };
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>
                                                    {
                                                        new Policy { VehicleId = vehicle.Id, IsValid = false },
                                                    }.AsQueryable());
            var service = new PolicyService(repository.Object);
            int existingVehicleId = 1;
            Assert.False(service.IsVehicleInsured(existingVehicleId));
        }

        [Fact]
        public void IsVehicleInsuredShouldReturnTrueOnExistingInsuredVehicle()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var vehicle = new Vehicle { Id = 1 };
            repository.Setup(r => r.All()).Returns(new List<Policy>
                                                    {
                                                        new Policy { VehicleId = vehicle.Id, IsValid = true },
                                                    }.AsQueryable());
            var service = new PolicyService(repository.Object);
            int existingVehicleId = 1;
            Assert.True(service.IsVehicleInsured(existingVehicleId));
        }

        [Fact]
        public void GetPremiumShouldReturnZeroOnZeroInsuranceSum()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var service = new PolicyService(repository.Object);
            Assert.Equal(0, service.GetPremium(0, "31-12-2008", "Car"));
        }

        [Fact]
        public void GetPremiumShouldReturnZeroOnNegtiveInsuranceSum()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var service = new PolicyService(repository.Object);
            Assert.Equal(0, service.GetPremium(-1000, "31-12-2008", "Car"));
        }

        [Fact]
        public void GetPremiumShouldReturnCorrectBasePremiumUpTo3Years()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var service = new PolicyService(repository.Object);
            Assert.Equal(50, service.GetPremium(1000, "31-12-2018", "Car"));
        }

        [Fact]
        public void GetPremiumShouldReturnCorrectBasePremiumUpTo10Years()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var service = new PolicyService(repository.Object);
            Assert.Equal(60, service.GetPremium(1000, "31-12-2014", "Car"));
        }

        [Fact]
        public void GetPremiumShouldReturnCorrectBasePremiumOver10Yearsa()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var service = new PolicyService(repository.Object);
            Assert.Equal(70, service.GetPremium(1000, "31-12-2000", "Car"));
        }

        [Fact]
        public void GetPremiumShouldReturnZeroForInvalidDataFormat()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var service = new PolicyService(repository.Object);
            Assert.Equal(0, service.GetPremium(1000, "invalidFormat", "Car"));
        }

        [Fact]
        public void GetPremiumShouldUseCorrectMultiplierForVehicleTypeTruck()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var service = new PolicyService(repository.Object);
            Assert.Equal(42, service.GetPremium(1000, "31-12-2000", "Truck"));
        }

        [Fact]
        public void GetPremiumShouldUseCorrectMultiplierForVehicleTypeMotorcycle()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var service = new PolicyService(repository.Object);
            Assert.Equal(105, service.GetPremium(1000, "31-12-2000", "Motorcycle"));
        }

        [Fact]
        public void GetPremiumShouldReturnZeroForInvalidVehicleType()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            var service = new PolicyService(repository.Object);
            Assert.Equal(0, service.GetPremium(1000, "31-12-2000", "InvalidType"));
        }

        [Fact]
        public void GetMyPoliciesShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>().AsQueryable());
            var service = new PolicyService(repository.Object);
            string clientId = "test";
            Assert.Equal(0, service.GetMyPolicies(clientId).Count);
        }

        [Fact]
        public void GetMyPoliciesShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>
                                                    {
                                                        new Policy { ClientId = "Pesho" },
                                                        new Policy { ClientId = "test" },
                                                    }.AsQueryable());
            var service = new PolicyService(repository.Object);
            string clientId = "test";
            Assert.Equal(1, service.GetMyPolicies(clientId).Count);
        }

        [Fact]
        public void GetMyPoliciesIssuedShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>().AsQueryable());
            var service = new PolicyService(repository.Object);
            string agentId = "test";
            Assert.Equal(0, service.GetMyPoliciesIssued(agentId).Count);
        }

        [Fact]
        public void GetMyPoliciesIssuedShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>
                                                    {
                                                        new Policy { AgentId = "Pesho" },
                                                        new Policy { AgentId = "test" },
                                                    }.AsQueryable());
            var service = new PolicyService(repository.Object);
            string clientId = "test";
            Assert.Equal(1, service.GetMyPoliciesIssued(clientId).Count);
        }

        [Fact]
        public void GetAllActivePoliciesShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>().AsQueryable());
            var service = new PolicyService(repository.Object);
            Assert.Equal(0, service.GetAllActivePolicies().Count);
        }

        [Fact]
        public void GetAllActivePoliciesShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>
                                                    {
                                                        new Policy { IsValid = false },
                                                        new Policy { IsValid = true },
                                                        new Policy { IsValid = true },
                                                    }.AsQueryable());
            var service = new PolicyService(repository.Object);
            Assert.Equal(2, service.GetAllActivePolicies().Count);
        }

        [Fact]
        public void GetAllExpiredPoliciesShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>().AsQueryable());
            var service = new PolicyService(repository.Object);
            Assert.Equal(0, service.GetAllExpiredPolicies().Count);
        }

        [Fact]
        public void GetAllExpiredPoliciesShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>
                                                    {
                                                        new Policy { IsValid = false },
                                                        new Policy { IsValid = false },
                                                        new Policy { IsValid = true },
                                                    }.AsQueryable());
            var service = new PolicyService(repository.Object);
            Assert.Equal(2, service.GetAllExpiredPolicies().Count);
        }

        [Fact]
        public void GetByIdShouldReturnNullOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>().AsQueryable());
            var service = new PolicyService(repository.Object);
            string randomId = "rndId";
            Assert.Null(service.GetById(randomId));
        }

        [Fact]
        public void GetByIdShouldReturnNullOnNonExistingId()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>
                                                    {
                                                        new Policy { Id = "ABC" },
                                                    }.AsQueryable());
            var service = new PolicyService(repository.Object);
            string nonExistingId = "test";
            Assert.Null(service.GetById(nonExistingId));
        }

        [Fact]
        public void GetByIdShouldReturnNotNullOnExistingId()
        {
            var repository = new Mock<IDeletableEntityRepository<Policy>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Policy>
                                                    {
                                                        new Policy { Id = "ABC" },
                                                    }.AsQueryable());
            var service = new PolicyService(repository.Object);
            string existingId = "ABC";
            var result = service.GetById(existingId);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TerminatePolicyShouldReturnFalseOnNonExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb1")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Policies.Add(new Policy { Id = "ABC", IsValid = true });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Policy>(dbContext);
            var service = new PolicyService(repository);
            string nonExistingId = "test";
            Assert.False(await service.TerminatePolicy(nonExistingId));
        }

        [Fact]
        public async Task TerminatePolicyShouldReturnFalseOnExistingIdButInvalidPolicy()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb2")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Policies.Add(new Policy { Id = "ABC", IsValid = false });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Policy>(dbContext);
            var service = new PolicyService(repository);
            string existingId = "ABC";
            Assert.False(await service.TerminatePolicy(existingId));
        }

        [Fact]
        public async Task TerminatePolicyShouldReturnTrueOnExistingIdValidPolicy()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb3")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Policies.Add(new Policy { Id = "ABC", IsValid = true });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Policy>(dbContext);
            var service = new PolicyService(repository);
            string existingId = "ABC";
            Assert.True(await service.TerminatePolicy(existingId));
        }

        [Fact]
        public async Task CreateShouldIncreaseCountOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb4")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Policy>(dbContext);
            var service = new PolicyService(repository);
            var newPolicy = new Policy();
            Assert.Equal(0, repository.All().Count());
            await service.Create(newPolicy);
            Assert.Equal(1, repository.All().Count());
        }
    }
}
