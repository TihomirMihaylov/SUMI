namespace SUMI.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Services.Data.Damages;
    using SUMI.Web.ViewModels;
    using SUMI.Web.ViewModels.Damages;

    [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
    public class DamagesController : BaseController
    {
        private readonly IDamageService damagesService;

        public DamagesController(IDamageService damagesService)
        {
            this.damagesService = damagesService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(DamagesCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var errorModel = new ErrorViewModel() { Message = "There was an error processing your request. Please try again." };
                return this.View("../Shared/Error", errorModel);
            }

            var newDamage = Mapper.Map<Damage>(inputModel);
            await this.damagesService.Add(newDamage);
            return this.Redirect("/Claims/Details/" + inputModel.ClaimId);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var claimId = await this.damagesService.RemoveDamage(id);
            if (claimId == -1)
            {
                var errorModel = new ErrorViewModel() { Message = "Invalid request" };
                return this.View("../Shared/Error", errorModel);
            }

            return this.Redirect("/Claims/Details/" + claimId);
        }
    }
}
