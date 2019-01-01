namespace SUMI.Web.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Services.Data.Damages;
    using SUMI.Web.Areas.Administrator.ViewModels;
    using SUMI.Web.Controllers;
    using SUMI.Web.ViewModels;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administrator")]
    public class DamagesController : BaseController
    {
        private readonly IDamageService damagesService;

        public DamagesController(IDamageService damagesService)
        {
            this.damagesService = damagesService;
        }

        public async Task<IActionResult> Settle(DamageSettlementInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var errorModel = new ErrorViewModel() { Message = "There was an error processing your request. Please try again." };
                return this.View("../Shared/Error", errorModel);
            }

            await this.damagesService.SettleDamage(inputModel.DamageId, inputModel.EstimatedCost);
            return this.Redirect("/Claims/Details/" + inputModel.ClaimId);
        }
    }
}
