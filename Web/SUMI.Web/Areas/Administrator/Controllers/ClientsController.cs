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

        public IActionResult All(int? pageNumber)
        {
            var model = this.clientService
                .GetAll()
                .OrderBy(c => c.FirstName)
                .Select(c => Mapper.Map<ClientsViewModel>(c)).ToList();

            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<ClientsViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }
    }
}
