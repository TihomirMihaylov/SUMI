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
    using SUMI.Data.Repositories;
    using SUMI.Services.Data.Clients;
    using Xunit;

    public class ClientServiceTests
    {
        [Fact]
        public void GetAllShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Client>>();
            repository.Setup(r => r.All()).Returns(new List<Client>().AsQueryable());
            var service = new ClientService(repository.Object);
            Assert.Equal(0, service.GetAll().Count);
        }

        [Fact]
        public void GetAllShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<Client>>();
            repository.Setup(r => r.All()).Returns(new List<Client>
                                                      {
                                                        new Client("Ivan", "Ivanov", "9508256790", new DateTime(1995, 8, 25)),
                                                        new Client("Maria", "Ivanova", "9608256790", new DateTime(1996, 8, 25)),
                                                      }.AsQueryable());
            var service = new ClientService(repository.Object);
            Assert.Equal(2, service.GetAll().Count);
        }

        [Fact]
        public async Task GetClientIdShouldReturnCorrectIdIfClientExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb1")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Clients.Add(new Client("Ivan", "Ivanov", "9508256790", new DateTime(1995, 8, 25)) { Id = "test" });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Client>(dbContext);
            var service = new ClientService(repository);
            string result = await service.GetClientId("Ivan", "Ivanov", "9508256790", new DateTime(1995, 8, 25));
            Assert.Equal("test", result);
            Assert.Equal(1, service.GetAll().Count);
        }

        [Fact]
        public async Task GetClientIdShouldCreateNewClientAndReturnNewlyCreatedIdIfClientDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb2")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Clients.Add(new Client("Ivan", "Ivanov", "9508256790", new DateTime(1995, 8, 25)) { Id = "test" });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Client>(dbContext);
            var service = new ClientService(repository);
            string result = await service.GetClientId("Maria", "Ivanova", "9608256790", new DateTime(1996, 8, 25));
            Assert.NotEqual("test", result);
            Assert.Equal(2, service.GetAll().Count);
        }
    }
}
