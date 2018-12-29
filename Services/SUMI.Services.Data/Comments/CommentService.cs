namespace SUMI.Services.Data.Comments
{
    using System.Threading.Tasks;

    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;

    public class CommentService : ICommentService
    {
        private readonly IDeletableEntityRepository<Comment> commentRepo;

        public CommentService(IDeletableEntityRepository<Comment> commentRepo)
        {
            this.commentRepo = commentRepo;
        }

        public async Task Create(Comment comment)
        {
            this.commentRepo.Add(comment);
            await this.commentRepo.SaveChangesAsync();
        }

        public async Task RemoveComment(int id)
        {
            var comment = await this.commentRepo.GetByIdAsync(id);
            if (comment != null)
            {
                this.commentRepo.Delete(comment);
                await this.commentRepo.SaveChangesAsync();
            }
        }
    }
}
