namespace SUMI.Services.Data.Comments
{
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface ICommentService
    {
        Task Create(Comment comment);
    }
}
