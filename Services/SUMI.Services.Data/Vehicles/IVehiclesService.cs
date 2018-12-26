﻿namespace SUMI.Services.Data.Vehicles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SUMI.Data.Models;
    using SUMI.Services.Data.ViewModels;

    public interface IVehiclesService
    {
        Task Create(Vehicle vehicle);

        bool VihicleExists(string vehecleIdentificationNumber);

        IList<VehicleViewModel> GetMyVehicles(string clientId);

        Task<Vehicle> GetByIdAsync(int id);

        IList<VehicleViewModel> GetAll();

        Task Edit(VehicleEditViewModel inputModel);

        Task Delete(int id);

        Task<string> GetNewClientId(Client client);
    }
}
