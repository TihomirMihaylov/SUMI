﻿namespace SUMI.Web.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Services.Data.Vehicles;
    using SUMI.Web.Areas.Administrator.ViewModels;
    using SUMI.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
    [Area("Administrator")]
    public class VehiclesController : BaseController
    {
        private readonly IVehiclesService vehiclesService;

        public VehiclesController(IVehiclesService vehiclesService)
        {
            this.vehiclesService = vehiclesService;
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
                return this.BadRequest("This vehicle already exists in database.");
            }

            var newClient = Mapper.Map<Client>(inputModel);
            string newClientId = await this.vehiclesService.GetNewClientId(newClient);

            var newVehicle = Mapper.Map<Vehicle>(inputModel);
            newVehicle.OwnerId = newClientId;
            await this.vehiclesService.Create(newVehicle);

            // await this.vehiclesService.AddVehicleWith(newVehicle);
            return this.Redirect("/");
        }
    }
}