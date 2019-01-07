namespace SUMI.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SUMI.Data;
    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;
    using SUMI.Data.Repositories;
    using SUMI.Services.Data.Vehicles;
    using SUMI.Services.Data.ViewModels;
    using Xunit;

    public class VehiclesServiceTests
    {
        [Fact]
        public void VehicleExistsShouldReturnFalseOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>().AsQueryable());
            var service = new VehiclesService(repository.Object);
            string randomVIN = "WAUZZZ1KZ8N123456";
            Assert.False(service.VihicleExists(randomVIN));
            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public void VehicleExistsShouldReturnFalseOnNonExistingVIN()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                        {
                                                            new Vehicle() { VIN = "ZFA131BAR00000000" },
                                                        }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            string nonExistingVIN = "WAUZZZ1KZ8N123456";
            Assert.False(service.VihicleExists(nonExistingVIN));
        }

        [Fact]
        public void VehicleExistsShouldReturnTrueOnExistingVIN()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                        {
                                                            new Vehicle() { VIN = "WAUZZZ1KZ8N123456" },
                                                        }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            string existingVIN = "WAUZZZ1KZ8N123456";
            Assert.True(service.VihicleExists(existingVIN));
        }

        [Fact]
        public void GetByIdShouldReturnNullOnEmptyCollection()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>().AsQueryable());
            var service = new VehiclesService(repository.Object);
            int randomId = 1;
            Assert.Null(service.GetById(randomId));
        }

        [Fact]
        public void GetByIdShouldReturnNullOnNonExistingId()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                        {
                                                            new Vehicle() { Id = 2 },
                                                        }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            int nonExistingId = 1;
            Assert.Null(service.GetById(nonExistingId));
        }

        [Fact]
        public void GetByIdShouldReturnNotNullOnExistingId()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.AllWithDeleted()).Returns(new List<Vehicle>
                                                        {
                                                            new Vehicle() { Id = 1 },
                                                        }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            int existingId = 1;
            Assert.NotNull(service.GetById(existingId));
        }

        [Fact]
        public void GetAllShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>().AsQueryable());
            var service = new VehiclesService(repository.Object);
            Assert.Equal(0, service.GetAll().Count);
        }

        [Fact]
        public void GetAllShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                        {
                                                            new Vehicle(),
                                                            new Vehicle(),
                                                        }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            Assert.Equal(2, service.GetAll().Count);
        }

        [Fact]
        public void GetMyVehiclesShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>().AsQueryable());
            var service = new VehiclesService(repository.Object);
            string randomClientId = "test";
            Assert.Equal(0, service.GetMyVehicles(randomClientId).Count);
        }

        [Fact]
        public void GetMyVehiclesShouldReturnCorrectCountOnNonEmptyRepositoryIfVehicleDoesNotExist()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                        {
                                                            new Vehicle() { OwnerId = "Pesho" },
                                                            new Vehicle() { OwnerId = "Gosho" },
                                                        }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            string nonExistingClientId = "test";
            Assert.Equal(0, service.GetMyVehicles(nonExistingClientId).Count);
        }

        [Fact]
        public void GetMyVehiclesShouldReturnCorrectCountOnNonEmptyRepositoryIfVehicleExist()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                        {
                                                            new Vehicle() { OwnerId = "Pesho" },
                                                            new Vehicle() { OwnerId = "Gosho" },
                                                        }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            string existingClientId = "Pesho";
            Assert.Equal(1, service.GetMyVehicles(existingClientId).Count);
        }

        [Fact]
        public void GetByVinShouldReturnNullOnEmptyCollection()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>().AsQueryable());
            var service = new VehiclesService(repository.Object);
            string randomVIN = "WAUZZZ1KZ8N123456";
            Assert.Null(service.GetByVin(randomVIN));
        }

        [Fact]
        public void GetByVinShouldReturnNullOnOnNonExistingVIN()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                        {
                                                            new Vehicle() { VIN = "ZFA131BAR00000000" },
                                                        }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            string nonExistingVIN = "WAUZZZ1KZ8N123456";
            Assert.Null(service.GetByVin(nonExistingVIN));
        }

        [Fact]
        public void GetByVinShouldReturnNotNullOnExistingVIN()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                        {
                                                            new Vehicle() { VIN = "WAUZZZ1KZ8N123456" },
                                                        }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            string existingVIN = "WAUZZZ1KZ8N123456";
            Assert.NotNull(service.GetByVin(existingVIN));
        }

        [Fact]
        public void CheckForValidInsuranceShouldReturnNullOnEmptyCollection()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            var service = new VehiclesService(repository.Object);
            var vehicle = new Vehicle
            {
                Policies = new List<Policy>(),
            };
            Assert.Null(service.CheckForValidInsurance(vehicle));
        }

        [Fact]
        public void CheckForValidInsuranceShouldReturnNullOnCollectionWithInvalidPolicies()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            var service = new VehiclesService(repository.Object);
            string policyId = "test";
            var vehicle = new Vehicle
            {
                Policies = new List<Policy>
                {
                    new Policy { Id = policyId, IsValid = false },
                },
            };
            Assert.Null(service.CheckForValidInsurance(vehicle));
        }

        [Fact]
        public void CheckForValidInsuranceShouldReturnCorrectString()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            var service = new VehiclesService(repository.Object);
            string policyId = "test";
            var vehicle = new Vehicle
            {
                Policies = new List<Policy>
                {
                    new Policy { Id = policyId, IsValid = true },
                },
            };
            Assert.Equal(policyId, service.CheckForValidInsurance(vehicle));
        }

        [Fact]
        public async Task CreateShouldIncreaseCountOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb1")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Vehicle>(dbContext);
            var service = new VehiclesService(repository);
            var newVehicle = new Vehicle();
            Assert.Equal(0, service.GetAll().Count);
            await service.Create(newVehicle);
            Assert.Equal(1, service.GetAll().Count);
        }

        [Fact]
        public async Task CreateShouldAddTheCorrectObject()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb2")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Vehicle>(dbContext);
            var service = new VehiclesService(repository);
            var newVehicle = new Vehicle { Id = 1, Make = "Mercedes", Model = "S500" };
            await service.Create(newVehicle);
            var vehicleFromDb = service.GetById(newVehicle.Id);
            Assert.Equal<Vehicle>(newVehicle, vehicleFromDb);
        }

        [Fact]
        public async Task DeleteShouldDoNothingOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb3")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Vehicle>(dbContext);
            var service = new VehiclesService(repository);
            int randomId = 1;
            await service.Delete(randomId);
            Assert.Equal(0, service.GetAll().Count);
        }

        [Fact]
        public async Task DeleteShouldDoNothingWithNonExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb4")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Vehicles.Add(new Vehicle { Id = 1 });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Vehicle>(dbContext);
            var service = new VehiclesService(repository);
            int nonExistingId = 2;
            await service.Delete(nonExistingId);
            Assert.Equal(1, service.GetAll().Count);
        }

        [Fact]
        public async Task DeleteShouldReduceCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb5")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Vehicles.Add(new Vehicle { Id = 1 });
            dbContext.Vehicles.Add(new Vehicle { Id = 2 });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Vehicle>(dbContext);
            var service = new VehiclesService(repository);
            int existingId = 2;
            await service.Delete(existingId);
            Assert.Equal(1, service.GetAll().Count);
        }

        [Fact]
        public async Task EditShouldDoNothingOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb6")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Vehicle>(dbContext);
            var service = new VehiclesService(repository);
            var inputModel = new VehicleEditViewModel
            {
                Id = 1,
            };
            await service.Edit(inputModel);
            Assert.Equal(0, service.GetAll().Count);
        }

        [Fact]
        public async Task EditShouldDoNothingWithNonExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb7")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Vehicles.Add(new Vehicle { Id = 1, Make = "Mercedes" });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Vehicle>(dbContext);
            var service = new VehiclesService(repository);
            var inputModel = new VehicleEditViewModel
            {
                Id = 2,
                Make = "BMW",
            };
            await service.Edit(inputModel);
            Assert.Equal("Mercedes", service.GetById(1).Make);
        }

        [Fact]
        public async Task EditShouldUpdateEntityWithExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb8")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var newVehicle = new Vehicle
            {
                Id = 1,
                Make = "Mercedes",
                Model = "S500",
                VIN = "WDB2221KZ8N654321",
                NumberPlate = "CA1111PA",
                FirstRegistration = new DateTime(2019, 1, 1),
                Type = VehicleType.Truck,
            };
            dbContext.Vehicles.Add(newVehicle);
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Vehicle>(dbContext);
            var service = new VehiclesService(repository);
            var inputModel = new VehicleEditViewModel
            {
                Id = 1,
                Make = "BMW",
                Model = "760Li",
                VIN = "WBNX6X6D30300G980",
                NumberPlate = "CA2222PA",
                FirstRegistration = "31-12-2018",
                Type = "Car",
            };
            await service.Edit(inputModel);
            Assert.Equal("BMW", service.GetById(1).Make);
            Assert.Equal("760Li", service.GetById(1).Model);
            Assert.Equal("WBNX6X6D30300G980", service.GetById(1).VIN);
            Assert.Equal("CA2222PA", service.GetById(1).NumberPlate);
            var expectedDate = new DateTime(2018, 12, 31);
            Assert.Equal<DateTime>(expectedDate, service.GetById(1).FirstRegistration);
            Assert.Equal<VehicleType>(VehicleType.Car, service.GetById(1).Type);
        }

        [Fact]
        public void CheckOwnershipShouldReturnFalseOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            repository.Setup(r => r.All()).Returns(new List<Vehicle>().AsQueryable());
            var service = new VehiclesService(repository.Object);
            string randomUserId = "testId";
            int randomVehicleId = 1;
            Assert.False(service.CheckOwnership(randomUserId, randomVehicleId));
        }

        [Fact]
        public void CheckOwnershipShouldReturnFalseOnIncorrectVehicleId()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            string randomUserId = "testId";
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                    {
                                                        new Vehicle { Id = 1, OwnerId = randomUserId },
                                                    }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            int nonExistingVehicleId = 2;
            Assert.False(service.CheckOwnership(randomUserId, nonExistingVehicleId));
        }

        [Fact]
        public void CheckOwnershipShouldReturnTrueOnCorrectVehicleId()
        {
            var repository = new Mock<IDeletableEntityRepository<Vehicle>>();
            string randomUserId = "testId";
            int existingVehicleId = 1;
            repository.Setup(r => r.All()).Returns(new List<Vehicle>
                                                    {
                                                        new Vehicle { Id = existingVehicleId, OwnerId = randomUserId },
                                                    }.AsQueryable());
            var service = new VehiclesService(repository.Object);
            Assert.True(service.CheckOwnership(randomUserId, existingVehicleId));
        }
    }
}
