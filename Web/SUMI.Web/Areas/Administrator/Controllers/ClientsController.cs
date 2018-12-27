namespace SUMI.Web.Areas.Administrator.Controllers
{
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Services.Data.Clients;
    using SUMI.Web.Areas.Administrator.ViewModels;
    using SUMI.Web.Controllers;
    using X.PagedList;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administrator")]
    public class ClientsController : BaseController
    {
        private readonly IClientService clientService;

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        public IActionResult All(int? page)
        {
            var model = this.clientService
                .GetAll()
                .OrderBy(c => c.FirstName)
                .Select(c => Mapper.Map<ClientsViewModel>(c)).ToList();

            int nextPage = page ?? 1;
            IPagedList<ClientsViewModel> pagedViewModels = model.ToPagedList(nextPage, 5);

            return this.View(pagedViewModels);
        }
    }
}
