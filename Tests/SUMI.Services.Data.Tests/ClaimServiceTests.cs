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
    using SUMI.Data.Models.Enums;
    using SUMI.Data.Repositories;
    using SUMI.Services.Data.Claims;
    using Xunit;

    public class ClaimServiceTests
    {
        [Fact]
        public void GetMyClaimsShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>().AsQueryable());
            var service = new ClaimService(repository.Object);
            string clientId = "test";
            Assert.Equal(0, service.GetMyClaims(clientId).Count);
        }

        [Fact]
        public void GetMyClaimsShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            var peshosPolicy = new Policy { ClientId = "Pesho" };
            var policy = new Policy { ClientId = "test" };
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>
                                                    {
                                                        new InsuranceClaim { Policy = peshosPolicy },
                                                        new InsuranceClaim { Policy = policy },
                                                    }.AsQueryable());
            var service = new ClaimService(repository.Object);
            string clientId = "test";
            Assert.Equal(1, service.GetMyClaims(clientId).Count);
        }

        [Fact]
        public void GetMyOpenClaimsShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>().AsQueryable());
            var service = new ClaimService(repository.Object);
            string agentId = "test";
            Assert.Equal(0, service.GetMyOpenClaims(agentId).Count);
        }

        [Fact]
        public void GetMyOpenClaimsShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>
                                                    {
                                                        new InsuranceClaim { AgentId = "Pesho", Status = ClaimStatus.Open },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Open },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Pending },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Settled },
                                                    }.AsQueryable());
            var service = new ClaimService(repository.Object);
            string agentId = "test";
            Assert.Equal(1, service.GetMyOpenClaims(agentId).Count);
        }

        [Fact]
        public void GetMyPendingClaimsShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>().AsQueryable());
            var service = new ClaimService(repository.Object);
            string agentId = "test";
            Assert.Equal(0, service.GetMyPendingClaims(agentId).Count);
        }

        [Fact]
        public void GetMyPendingClaimsShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>
                                                    {
                                                        new InsuranceClaim { AgentId = "Pesho", Status = ClaimStatus.Pending },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Open },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Pending },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Settled },
                                                    }.AsQueryable());
            var service = new ClaimService(repository.Object);
            string agentId = "test";
            Assert.Equal(1, service.GetMyPendingClaims(agentId).Count);
        }

        [Fact]
        public void GetMyResolvedClaimsShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>().AsQueryable());
            var service = new ClaimService(repository.Object);
            string agentId = "test";
            Assert.Equal(0, service.GetMyResolvedClaims(agentId).Count);
        }

        [Fact]
        public void GetMyResolvedClaimsShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>
                                                    {
                                                        new InsuranceClaim { AgentId = "Pesho", Status = ClaimStatus.Settled },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Open },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Pending },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Settled },
                                                    }.AsQueryable());
            var service = new ClaimService(repository.Object);
            string agentId = "test";
            Assert.Equal(1, service.GetMyResolvedClaims(agentId).Count);
        }

        [Fact]
        public void GetAllPendingClaimsShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>().AsQueryable());
            var service = new ClaimService(repository.Object);
            Assert.Equal(0, service.GetAllPendingClaims().Count);
        }

        [Fact]
        public void GetAllPendingClaimsShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>
                                                    {
                                                        new InsuranceClaim { AgentId = "Pesho", Status = ClaimStatus.Pending },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Open },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Pending },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Settled },
                                                    }.AsQueryable());
            var service = new ClaimService(repository.Object);
            Assert.Equal(2, service.GetAllPendingClaims().Count);
        }

        [Fact]
        public void GetAllResolvedClaimsShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>().AsQueryable());
            var service = new ClaimService(repository.Object);
            Assert.Equal(0, service.GetAllResolvedClaims().Count);
        }

        [Fact]
        public void GetAllResolvedClaimsShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>
                                                    {
                                                        new InsuranceClaim { AgentId = "Pesho", Status = ClaimStatus.Settled },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Open },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Pending },
                                                        new InsuranceClaim { AgentId = "test", Status = ClaimStatus.Settled },
                                                    }.AsQueryable());
            var service = new ClaimService(repository.Object);
            Assert.Equal(2, service.GetAllResolvedClaims().Count);
        }

        [Fact]
        public void GetAllUnsettledByPolicyIdShouldReturnEmptyListOnEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>().AsQueryable());
            var service = new ClaimService(repository.Object);
            string policyId = "test";
            Assert.Equal(0, service.GetAllUnsettledByPolicyId(policyId).Count);
        }

        [Fact]
        public void GetAllUnsettledByPolicyIdShouldReturnCorrectCountOnNonEmptyRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>
                                                    {
                                                        new InsuranceClaim { PolicyId = "Pesho", Status = ClaimStatus.Open },
                                                        new InsuranceClaim { PolicyId = "Pesho", Status = ClaimStatus.Pending },
                                                        new InsuranceClaim { PolicyId = "test", Status = ClaimStatus.Open },
                                                        new InsuranceClaim { PolicyId = "test", Status = ClaimStatus.Pending },
                                                        new InsuranceClaim { PolicyId = "test", Status = ClaimStatus.Settled },
                                                    }.AsQueryable());
            var service = new ClaimService(repository.Object);
            string policyId = "test";
            Assert.Equal(2, service.GetAllUnsettledByPolicyId(policyId).Count);
        }

        [Fact]
        public async Task ChangeStatusToPendingShouldDoNothingOnNonExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb1")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Claims.Add(new InsuranceClaim { Id = 1, Status = ClaimStatus.Open });
            dbContext.Claims.Add(new InsuranceClaim { Id = 2, Status = ClaimStatus.Pending });
            dbContext.Claims.Add(new InsuranceClaim { Id = 3, Status = ClaimStatus.Settled });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<InsuranceClaim>(dbContext);
            var service = new ClaimService(repository);
            int nonExistingId = 4;
            await service.ChangeStatusToPending(nonExistingId);
            Assert.True((await repository.GetByIdAsync(1)).Status == ClaimStatus.Open);
            Assert.True((await repository.GetByIdAsync(2)).Status == ClaimStatus.Pending);
            Assert.True((await repository.GetByIdAsync(3)).Status == ClaimStatus.Settled);
        }

        [Fact]
        public async Task ChangeStatusToPendingShouldChangeStatusOnExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb2")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Claims.Add(new InsuranceClaim { Id = 1, Status = ClaimStatus.Open });
            dbContext.Claims.Add(new InsuranceClaim { Id = 2, Status = ClaimStatus.Settled });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<InsuranceClaim>(dbContext);
            var service = new ClaimService(repository);
            await service.ChangeStatusToPending(1);
            Assert.True((await repository.GetByIdAsync(1)).Status == ClaimStatus.Pending);
            await service.ChangeStatusToPending(2);
            Assert.True((await repository.GetByIdAsync(2)).Status == ClaimStatus.Pending);
        }

        [Fact]
        public async Task ChangeStatusToSettledShouldDoNothingOnNonExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb3")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Claims.Add(new InsuranceClaim { Id = 1, Status = ClaimStatus.Open });
            dbContext.Claims.Add(new InsuranceClaim { Id = 2, Status = ClaimStatus.Pending });
            dbContext.Claims.Add(new InsuranceClaim { Id = 3, Status = ClaimStatus.Settled });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<InsuranceClaim>(dbContext);
            var service = new ClaimService(repository);
            int nonExistingId = 4;
            await service.ChangeStatusToSettled(nonExistingId);
            Assert.True((await repository.GetByIdAsync(1)).Status == ClaimStatus.Open);
            Assert.True((await repository.GetByIdAsync(2)).Status == ClaimStatus.Pending);
            Assert.True((await repository.GetByIdAsync(3)).Status == ClaimStatus.Settled);
        }

        [Fact]
        public async Task ChangeStatusToSettledShouldChangeStatusOnExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb4")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Claims.Add(new InsuranceClaim { Id = 1, Status = ClaimStatus.Open });
            dbContext.Claims.Add(new InsuranceClaim { Id = 2, Status = ClaimStatus.Pending });
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<InsuranceClaim>(dbContext);
            var service = new ClaimService(repository);
            await service.ChangeStatusToSettled(1);
            Assert.True((await repository.GetByIdAsync(1)).Status == ClaimStatus.Settled);
            await service.ChangeStatusToSettled(2);
            Assert.True((await repository.GetByIdAsync(2)).Status == ClaimStatus.Settled);
        }

        [Fact]
        public async Task CreateShouldIncreaseCountOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb5")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<InsuranceClaim>(dbContext);
            var service = new ClaimService(repository);
            var newClaim = new InsuranceClaim();
            Assert.Equal(0, repository.All().Count());
            await service.Create(newClaim);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task CreateShouldAddTheCorrectObject()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb6")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<InsuranceClaim>(dbContext);
            var service = new ClaimService(repository);
            var newClaim = new InsuranceClaim { Id = 1, Description = "test" };
            await service.Create(newClaim);
            var claimFromDb = await repository.GetByIdAsync(newClaim.Id);
            Assert.Equal<InsuranceClaim>(newClaim, claimFromDb);
        }

        [Fact]
        public async Task DeleteShouldDoNothingOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb7")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<InsuranceClaim>(dbContext);
            var service = new ClaimService(repository);
            int randomId = 1;
            await service.Delete(randomId);
            Assert.Equal(0, repository.All().Count());
        }

        [Fact]
        public async Task DeleteShouldDoNothingWithNonExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb8")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var claim1 = new InsuranceClaim { Id = 1 };
            var claim2 = new InsuranceClaim { Id = 2 };
            dbContext.Claims.Add(claim1);
            dbContext.Claims.Add(claim2);
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<InsuranceClaim>(dbContext);
            var service = new ClaimService(repository);
            int nonExistingId = 3;
            await service.Delete(nonExistingId);
            Assert.Equal(2, repository.All().Count());
        }

        [Fact]
        public async Task DeleteShouldReduceCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb9")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var claim1 = new InsuranceClaim { Id = 1 };
            var claim2 = new InsuranceClaim { Id = 2 };
            dbContext.Claims.Add(claim1);
            dbContext.Claims.Add(claim2);
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<InsuranceClaim>(dbContext);
            var service = new ClaimService(repository);
            int existingId = 2;
            await service.Delete(existingId);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public void GetByIdShouldReturnNullOnEmptyCollection()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>().AsQueryable());
            var service = new ClaimService(repository.Object);
            int randomId = 1;
            Assert.Null(service.GetById(randomId));
        }

        [Fact]
        public void GetByIdShouldReturnNullOnNonExistingId()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>
                                                        {
                                                            new InsuranceClaim() { Id = 2 },
                                                        }.AsQueryable());
            var service = new ClaimService(repository.Object);
            int nonExistingId = 1;
            Assert.Null(service.GetById(nonExistingId));
        }

        [Fact]
        public void GetByIdShouldReturnNotNullOnExistingId()
        {
            var repository = new Mock<IDeletableEntityRepository<InsuranceClaim>>();
            repository.Setup(r => r.All()).Returns(new List<InsuranceClaim>
                                                        {
                                                            new InsuranceClaim() { Id = 1 },
                                                        }.AsQueryable());
            var service = new ClaimService(repository.Object);
            int existingId = 1;
            Assert.NotNull(service.GetById(existingId));
        }
    }
}
