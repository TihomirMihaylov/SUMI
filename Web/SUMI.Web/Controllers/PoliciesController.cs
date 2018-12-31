namespace SUMI.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Services.Data.Policies;
    using SUMI.Web.ViewModels;
    using SUMI.Web.ViewModels.Policies;
    using X.PagedList;

    [Authorize]
    public class PoliciesController : BaseController
    {
        private readonly IPolicyService policyService;
        private readonly UserManager<ApplicationUser> userManager;

        public PoliciesController(IPolicyService policyService, UserManager<ApplicationUser> userManager)
        {
            this.policyService = policyService;
            this.userManager = userManager;
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public IActionResult Create() => this.View();

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> Create(PolicyCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            if (this.policyService.IsVehicleInsured(inputModel.VehicleId))
            {
                var errorModel = new ErrorViewModel() { Message = "This vehicle has a valid policy." };
                return this.View("../Shared/Error", errorModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
            var newPolicy = Mapper.Map<Policy>(inputModel);
            newPolicy.AgentId = currentUser.Id;
            await this.policyService.Create(newPolicy);

            string id = newPolicy.Id;
            return this.RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public IActionResult Calculate(decimal insuranceSum, string firstRegistration, string type)
        {
            decimal premium = this.policyService.GetPremium(insuranceSum, firstRegistration, type);
            return this.Json(new { premium });
        }

        public async Task<IActionResult> MyPolicies()
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            IList<Policy> myPolicies = this.policyService.GetMyPolicies(currentUser.ClientId);
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            var model = myPolicies.Select(p => Mapper.Map<PolicyViewModel>(p)).ToList();
            return this.View(model);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> MyIssuedPolicies(int? page)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            IList<Policy> myPolicies = this.policyService.GetMyPoliciesIssued(currentUser.Id);
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            var model = myPolicies.Select(p => Mapper.Map<PolicyViewModel>(p)).ToList();

            int nextPage = page ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<PolicyViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        public IActionResult Details(string id)
        {
            var policy = this.policyService.GetById(id);
            var model = Mapper.Map<PolicyDetailsViewModel>(policy);
            var agent = this.userManager.Users.FirstOrDefault(u => u.Id == model.AgentId);
            model.Agent = agent;
            return this.View(model);
        }
    }
}
