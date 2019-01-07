namespace SUMI.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;
    using SUMI.Services.Data.Claims;
    using SUMI.Services.Data.Damages;
    using SUMI.Services.Data.Policies;
    using SUMI.Web.ViewModels;
    using SUMI.Web.ViewModels.Claims;
    using X.PagedList;

    [Authorize]
    public class ClaimsController : BaseController
    {
        private readonly IClaimService claimsService;
        private readonly IPolicyService policyService;
        private readonly IDamageService damageService;
        private readonly UserManager<ApplicationUser> userManager;

        public ClaimsController(IClaimService claimsService, IPolicyService policyService, IDamageService damageService, UserManager<ApplicationUser> userManager)
        {
            this.claimsService = claimsService;
            this.policyService = policyService;
            this.damageService = damageService;
            this.userManager = userManager;
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public IActionResult Create() => this.View();

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> Create(ClaimCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            if (!this.policyService.IsVehicleInsured(inputModel.VehicleId))
            {
                var errorModel = new ErrorViewModel() { Message = "This vehicle doesn't have a valid policy." };
                return this.View("../Shared/Error", errorModel);
            }

            var newClaim = Mapper.Map<InsuranceClaim>(inputModel);
            newClaim.Status = ClaimStatus.Open;
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
            newClaim.AgentId = currentUser.Id;
            await this.claimsService.Create(newClaim);

            int id = newClaim.Id;
            return this.RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> MyClaims(int? pageNumber)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            var myClaims = this.claimsService.GetMyClaims(currentUser.ClientId);
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            var model = myClaims.Select(v => Mapper.Map<ClaimViewModel>(v)).ToList();

            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<ClaimViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> MyOpen(int? pageNumber)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            var myOpenClaims = this.claimsService.GetMyOpenClaims(currentUser.Id);
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            var model = myOpenClaims.Select(v => Mapper.Map<ClaimViewModel>(v)).ToList();

            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<ClaimViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> MyPending(int? pageNumber)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            var myPendingClaims = this.claimsService.GetMyPendingClaims(currentUser.Id);
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            var model = myPendingClaims.Select(v => Mapper.Map<ClaimViewModel>(v)).ToList();

            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<ClaimViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> MyResolved(int? pageNumber)
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            var myResolvedClaims = this.claimsService.GetMyResolvedClaims(currentUser.Id);
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            var model = myResolvedClaims.Select(v => Mapper.Map<ClaimViewModel>(v)).ToList();

            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<ClaimViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.claimsService.Delete(id);
            return this.RedirectToAction(nameof(this.MyOpen));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (this.User.IsInRole(GlobalConstants.ClientRoleName))
            {
                var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
                bool isPolicyHolder = this.claimsService.CheckOwnership(currentUser.ClientId, id);
                if (!isPolicyHolder)
                {
                    return this.LocalRedirect("/Identity/Account/AccessDenied");
                }
            }

            if (this.User.IsInRole(GlobalConstants.AgentRoleName))
            {
                var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
                bool isClaimCreator = this.claimsService.CheckCreator(currentUser.Id, id);
                if (!isClaimCreator)
                {
                    return this.LocalRedirect("/Identity/Account/AccessDenied");
                }
            }

            var claim = this.claimsService.GetById(id);
            if (claim == null)
            {
                var errorModel = new ErrorViewModel() { Message = "Claim doesn't exist." };
                return this.View("../Shared/Error", errorModel);
            }

            var model = Mapper.Map<ClaimDetailsViewModel>(claim);
            var totalSpentForThisPolicy = this.damageService.GetTotalAmountSpentForPolicy(claim.PolicyId);
            model.TotalSpent = totalSpentForThisPolicy;
            return this.View(model);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> Send(int id)
        {
            await this.claimsService.ChangeStatusToPending(id);
            return this.Redirect("/");
        }
    }
}
