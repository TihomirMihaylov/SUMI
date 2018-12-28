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

            // Pagination doesn't work. The problem might be it doesn't map query parameters e.g. /all?page=2
            int nextPage = page ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            int entriesPerPage = 5;
            this.ViewBag.EntriesPerPage = entriesPerPage;
            IPagedList<ClientsViewModel> pagedViewModels = model.ToPagedList(nextPage, entriesPerPage);
            return this.View(pagedViewModels);
        }
    }
}
