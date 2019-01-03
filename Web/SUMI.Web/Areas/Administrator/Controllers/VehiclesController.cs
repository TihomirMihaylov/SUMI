namespace SUMI.Web.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Services.Data.Clients;
    using SUMI.Services.Data.Vehicles;
    using SUMI.Web.Areas.Administrator.ViewModels;
    using SUMI.Web.Controllers;
    using SUMI.Web.ViewModels;

    [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
    [Area("Administrator")]
    public class VehiclesController : BaseController
    {
        private readonly IVehiclesService vehiclesService;
        private readonly IClientService clientService;

        public VehiclesController(IVehiclesService vehiclesService, IClientService clientService)
        {
            this.vehiclesService = vehiclesService;
            this.clientService = clientService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleAndClientInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            if (this.vehiclesService.VihicleExists(inputModel.VIN))
            {
                var errorModel = new ErrorViewModel() { Message = "This vehicle already exists in database." };
                return this.View("../Shared/Error", errorModel);
            }

            var newClient = Mapper.Map<Client>(inputModel);
            string newClientId = await this.clientService.GetNewClientId(newClient);

            var newVehicle = Mapper.Map<Vehicle>(inputModel);
            newVehicle.OwnerId = newClientId;
            await this.vehiclesService.Create(newVehicle);

            return this.Redirect("/");
        }
    }
}
