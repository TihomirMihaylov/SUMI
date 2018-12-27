namespace SUMI.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Services.Data.Policies;

    [Authorize]
    public class PoliciesController : BaseController
    {
        private readonly IPolicyService policyService;

        public PoliciesController(IPolicyService policyService)
        {
            this.policyService = policyService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public IActionResult Create() => this.View();
    }
}
