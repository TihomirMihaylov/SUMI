﻿namespace SUMI.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Services.Data.Vehicles;
    using SUMI.Services.Data.ViewModels;
    using SUMI.Web.ViewModels;
    using SUMI.Web.ViewModels.Vehicles;
    using X.PagedList;

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

        [Authorize(Roles = GlobalConstants.ClientRoleName)]
        public IActionResult Create() => this.View();

        [HttpPost]
        [Authorize(Roles = GlobalConstants.ClientRoleName)]
        public async Task<IActionResult> Create(VehicleCreateInputModel inputModel)
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
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            IList<Vehicle> myVehicles = this.vehiclesService.GetMyVehicles(currentUser.ClientId);
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            var model = myVehicles.Select(v => Mapper.Map<VehicleViewModel>(v)).ToList();
            return this.View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (this.User.IsInRole(GlobalConstants.ClientRoleName))
            {
                var currentUser = await this.userManager.GetUserAsync(this.HttpContext.User);
                bool isOwner = this.vehiclesService.CheckOwnership(currentUser.ClientId, id);
                if (!isOwner)
                {
                    return this.LocalRedirect("/Identity/Account/AccessDenied");
                }
            }

            var vehicle = this.vehiclesService.GetById(id);
            if (vehicle == null)
            {
                var errorModel = new ErrorViewModel() { Message = "Vehicle doesn't exist." };
                return this.View("../Shared/Error", errorModel);
            }

            var model = Mapper.Map<VehicleDetailsViewModel>(vehicle);
            return this.View(model);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public IActionResult All(int? pageNumber)
        {
            var allVehicles = this.vehiclesService.GetAll();
            var model = allVehicles.Select(v => Mapper.Map<VehicleViewModel>(v)).ToList();

            // Pagination doesn't work. The problem might be it doesn't map query parameters e.g. /all?page=2
            int nextPage = pageNumber ?? 1;
            this.ViewBag.CurrentPage = nextPage;
            IPagedList<VehicleViewModel> pagedViewModels = model.ToPagedList(nextPage, GlobalConstants.EntriesPerPage);
            return this.View(pagedViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public IActionResult Edit(int id)
        {
            var vehicle = this.vehiclesService.GetById(id);
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
        public IActionResult Delete(int id)
        {
            var vehicle = this.vehiclesService.GetById(id);
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

        [Authorize(Roles = GlobalConstants.AdministratorOrAgent)]
        public IActionResult SearchByVin(string vin)
        {
            if (!this.vehiclesService.VihicleExists(vin))
            {
                return this.NotFound();
            }

            var vehicle = this.vehiclesService.GetByVin(vin);
            var viewModel = Mapper.Map<VehicleDetailsViewModel>(vehicle);
            viewModel.OwnerName = $"{vehicle.Owner.FirstName} {vehicle.Owner.LastName}";
            return this.Json(viewModel);
        }

        public IActionResult GetInsuranceStatus(string vin)
        {
            if (!this.vehiclesService.VihicleExists(vin))
            {
                bool vehicleExists = false;
                return this.Json(new { vehicleExists });
            }

            var vehicle = this.vehiclesService.GetByVin(vin);
            var policyId = this.vehiclesService.CheckForValidInsurance(vehicle);
            if (string.IsNullOrWhiteSpace(policyId))
            {
                bool isInsured = false;
                return this.Json(new { isInsured });
            }

            return this.Json(new { VehicleId = vehicle.Id, PolicyId = policyId });
        }
    }
}
