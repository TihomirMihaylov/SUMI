namespace SUMI.Web.Areas.Administrator.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Services.Data.Claims;
    using SUMI.Web.Controllers;
    using SUMI.Web.ViewModels.Claims;
    using X.PagedList;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administrator")]
    public class ClaimsController : BaseController
    {
        private readonly IClaimService claimsService;

        public ClaimsController(IClaimService claimsService)
        {
            this.claimsService = claimsService;
        }

        public IActionResult AllPending(int? pageNumber)
        {
            var pendingClaims = this.claimsService.GetAllPendingClaims();
            var model = pendingClaims.Select(v => Mapper.Map<ClaimViewModel>(v)).ToList();

            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<ClaimViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        public IActionResult AllResolved(int? pageNumber)
        {
            var resolvedClaims = this.claimsService.GetAllResolvedClaims();
            var model = resolvedClaims.Select(v => Mapper.Map<ClaimViewModel>(v)).ToList();

            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<ClaimViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        public async Task<IActionResult> Resolve(int id)
        {
            await this.claimsService.ChangeStatusToSettled(id);
            return this.RedirectToAction(nameof(this.AllPending));
        }
    }
}
