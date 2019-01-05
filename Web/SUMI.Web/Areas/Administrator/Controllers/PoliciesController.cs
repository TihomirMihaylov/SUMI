namespace SUMI.Web.Areas.Administrator.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Services.Data.Claims;
    using SUMI.Services.Data.Policies;
    using SUMI.Web.Controllers;
    using SUMI.Web.ViewModels.Policies;
    using X.PagedList;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administrator")]
    public class PoliciesController : BaseController
    {
        private readonly IPolicyService policyService;
        private readonly IClaimService claimService;

        public PoliciesController(IPolicyService policyService, IClaimService claimService)
        {
            this.policyService = policyService;
            this.claimService = claimService;
        }

        public IActionResult AllActive(int? pageNumber)
        {
            var activePolicies = this.policyService.GetAllActivePolicies();
            var model = activePolicies.Select(p => Mapper.Map<PolicyViewModel>(p)).ToList();

            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<PolicyViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        public IActionResult AllExpired(int? pageNumber)
        {
            var expiredPolicies = this.policyService.GetAllExpiredPolicies();
            var model = expiredPolicies.Select(p => Mapper.Map<PolicyViewModel>(p)).ToList();

            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<PolicyViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        public async Task<IActionResult> Terminate(string id)
        {
            var result = await this.policyService.TerminatePolicy(id);
            if (!result)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        public async Task<IActionResult> TotalLoss(string policyId)
        {
            var result = await this.policyService.TerminatePolicy(policyId);
            if (!result)
            {
                return this.NotFound();
            }

            var unsettledClaims = this.claimService.GetAllUnsettledByPolicyId(policyId);
            foreach (var claim in unsettledClaims)
            {
                await this.claimService.ChangeStatusToSettled(claim.Id);
            }

            return this.Ok();
        }
    }
}
