namespace SUMI.Web.Areas.Administrator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Services.Data.Policies;
    using SUMI.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administrator")]
    public class PoliciesController : BaseController
    {
        private readonly IPolicyService policyService;

        public PoliciesController(IPolicyService policyService)
        {
            this.policyService = policyService;
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
    }
}
