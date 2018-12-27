namespace SUMI.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using SUMI.Common;
    using SUMI.Data.Models;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IList<ApplicationUser>> GetAdmins(string currentUserId)
        {
            var allAdmins = await this.userManager.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName);
            var result = allAdmins.Where(u => u.Id != currentUserId).ToList();
            return result;
        }

        public async Task<IList<ApplicationUser>> GetAgents()
        {
            var result = await this.userManager.GetUsersInRoleAsync(GlobalConstants.AgentRoleName);
            return result;
        }

        public async Task<IList<ApplicationUser>> GetClients()
        {
            var result = await this.userManager.GetUsersInRoleAsync(GlobalConstants.ClientRoleName);
            return result;
        }
    }
}
