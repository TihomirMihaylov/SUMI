namespace SUMI.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Services.Data.Claims;
    using SUMI.Services.Data.Policies;
    using SUMI.Web.ViewModels;
    using SUMI.Web.ViewModels.Claims;

    [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
    public class ClaimsController : BaseController
    {
        private readonly IClaimsService claimsService;
        private readonly IPolicyService policyService;

        public ClaimsController(IClaimsService claimsService, IPolicyService policyService)
        {
            this.claimsService = claimsService;
            this.policyService = policyService;
        }

        public IActionResult Create() => this.View();

        [HttpPost]
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
            await this.claimsService.Create(newClaim);

            return this.Redirect("/");

            // int id = newClaim.Id;
            // return this.RedirectToAction("Details", new { id });
        }
    }
}
