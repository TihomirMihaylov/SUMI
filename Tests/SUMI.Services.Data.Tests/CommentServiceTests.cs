namespace SUMI.Services.Data.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SUMI.Data;
    using SUMI.Data.Models;
    using SUMI.Data.Repositories;
    using SUMI.Services.Data.Comments;
    using Xunit;

    public class CommentServiceTests
    {
        [Fact]
        public async Task CreateShouldIncreaseCountOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb1")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Comment>(dbContext);
            var service = new CommentService(repository);
            var newComment = new Comment();
            Assert.Equal(0, repository.All().Count());
            await service.Create(newComment);
            Assert.Equal(1, repository.All().Count());
        }

        [Fact]
        public async Task CreateShouldAddTheCorrectObject()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb2")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Comment>(dbContext);
            var service = new CommentService(repository);
            var newComment = new Comment { Id = 1, Text = "test" };
            await service.Create(newComment);
            var commentFromDb = await repository.GetByIdAsync(newComment.Id);
            Assert.Equal<Comment>(newComment, commentFromDb);
        }

        [Fact]
        public async Task RemoveCommentShouldDoNothingOnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb3")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Comment>(dbContext);
            var service = new CommentService(repository);
            int randomId = 1;
            await service.RemoveComment(randomId);
            Assert.Equal(0, repository.All().Count());
        }

        [Fact]
        public async Task RemoveCommentShouldDoNothingWithNonExistingId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb4")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var comment1 = new Comment { Id = 1 };
            var comment2 = new Comment { Id = 2 };
            dbContext.Comments.Add(comment1);
            dbContext.Comments.Add(comment2);
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Comment>(dbContext);
            var service = new CommentService(repository);
            int nonExistingId = 3;
            await service.RemoveComment(nonExistingId);
            Assert.Equal(2, repository.All().Count());
        }

        [Fact]
        public async Task RemoveCommentShouldReduceCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyTestDb5")
                .Options;
            var dbContext = new ApplicationDbContext(options);
            var comment1 = new Comment { Id = 1 };
            var comment2 = new Comment { Id = 2 };
            dbContext.Comments.Add(comment1);
            dbContext.Comments.Add(comment2);
            await dbContext.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Comment>(dbContext);
            var service = new CommentService(repository);
            int existingId = 2;
            await service.RemoveComment(existingId);
            Assert.Equal(1, repository.All().Count());
        }
    }
}
