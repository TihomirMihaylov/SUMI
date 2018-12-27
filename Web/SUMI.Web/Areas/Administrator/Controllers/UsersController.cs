namespace SUMI.Web.Areas.Administrator.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Services.Data.Users;
    using SUMI.Web.Areas.Administrator.ViewModels;
    using SUMI.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administrator")]
    public class UsersController : BaseController
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All()
        {
            var model = new AdministrationViewModel();

            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
            var admins = await this.userService.GetAdmins(currentUser.Id);
            var adminModels = admins.Select(x => Mapper.Map<UserViewModel>(x)).OrderBy(x => x.FirstName).ToList();
            model.Admins = adminModels;

            var agents = await this.userService.GetAgents();
            var agentModels = agents.Select(x => Mapper.Map<UserViewModel>(x)).OrderBy(x => x.FirstName).ToList();
            model.Agents = agentModels;

            var clients = await this.userService.GetClients();
            var clientModels = clients.Select(x => Mapper.Map<UserViewModel>(x)).OrderBy(x => x.FirstName).ToList();
            model.Clients = clientModels;

            return this.View(model);
        }
    }
}
