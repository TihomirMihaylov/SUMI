namespace SUMI.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;
    using SUMI.Services.Data.Vehicles;
    using SUMI.Services.Data.ViewModels;
    using SUMI.Web.ViewModels.Vehicles;

    [Authorize]
    public class VehiclesController : BaseController
    {
        private readonly IVehiclesService vehiclesService;
        private readonly UserManager<ApplicationUser> userManager;

        public VehiclesController(IVehiclesService vehiclesService, UserManager<ApplicationUser> userManager)
        {
            this.vehiclesService = vehiclesService;
            this.userManager = userManager;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            if (this.vehiclesService.VihicleExists(inputModel.VIN))
            {
                return this.BadRequest("This vehicle already exists in database.");
            }

            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
            var newVehicle = Mapper.Map<Vehicle>(inputModel);
            newVehicle.OwnerId = currentUser.ClientId;
            await this.vehiclesService.Create(newVehicle);

            int id = newVehicle.Id;
            return this.RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> MyVehicles()
        {
            var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
            var model = this.vehiclesService.GetMyVehicles(currentUser.ClientId).ToList();
            return this.View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await this.vehiclesService.GetByIdAsync(id);
            var model = Mapper.Map<VehicleDetailsViewModel>(vehicle);
            return this.View(model);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public IActionResult All()
        {
            var model = this.vehiclesService.GetAll().ToList();
            return this.View(model);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await this.vehiclesService.GetByIdAsync(id);
            var model = Mapper.Map<VehicleDetailsViewModel>(vehicle);
            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> Edit(VehicleEditViewModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.vehiclesService.Edit(inputModel);

            int id = inputModel.Id;
            return this.RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await this.vehiclesService.GetByIdAsync(id);
            var model = Mapper.Map<VehicleDetailsViewModel>(vehicle);
            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public async Task<IActionResult> Delete(VehicleEditViewModel inputModel)
        {
            await this.vehiclesService.Delete(inputModel.Id);
            return this.RedirectToAction("All");
        }
    }
}
