namespace SUMI.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IUserService
    {
        Task<IList<ApplicationUser>> GetAdmins(string currentUserId);

        Task<IList<ApplicationUser>> GetAgents();

        Task<IList<ApplicationUser>> GetClients();
    }
}
