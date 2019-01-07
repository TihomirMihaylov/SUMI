namespace SUMI.Services.Data.Vehicles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;
    using SUMI.Services.Data.ViewModels;

    public class VehiclesService : IVehiclesService
    {
        private readonly IDeletableEntityRepository<Vehicle> vehiclesRepo;

        public VehiclesService(IDeletableEntityRepository<Vehicle> vehiclesRepo)
        {
            this.vehiclesRepo = vehiclesRepo;
        }

        public async Task Create(Vehicle vehicle)
        {
            this.vehiclesRepo.Add(vehicle);
            await this.vehiclesRepo.SaveChangesAsync();
        } // Tested

        public bool VihicleExists(string vehecleIdentificationNumber)
        {
            return this.vehiclesRepo.All().Any(v => v.VIN == vehecleIdentificationNumber);
        } // Tested

        public IList<Vehicle> GetMyVehicles(string clientId)
            => this.vehiclesRepo.All()
                .Where(v => v.OwnerId == clientId)
                .OrderBy(v => v.Make)
                .ThenBy(v => v.Model)
                .ThenBy(v => v.NumberPlate)
                .ToList(); // Tested

        public Vehicle GetById(int id)
            => this.vehiclesRepo.AllWithDeleted()
                .Include(v => v.Owner)
                .FirstOrDefault(v => v.Id == id); // Tested

        public IList<Vehicle> GetAll()
            => this.vehiclesRepo.All()
                .OrderBy(v => v.Make)
                .ThenBy(v => v.Model)
                .ThenBy(v => v.NumberPlate)
                .ToList(); // Tested

        public async Task Edit(VehicleEditViewModel inputModel)
        {
            var vehicleToUpdate = await this.vehiclesRepo.GetByIdAsync(inputModel.Id);
            if (vehicleToUpdate != null)
            {
                vehicleToUpdate.Make = inputModel.Make;
                vehicleToUpdate.Model = inputModel.Model;
                vehicleToUpdate.VIN = inputModel.VIN;
                vehicleToUpdate.NumberPlate = inputModel.NumberPlate;
                if (!string.IsNullOrEmpty(inputModel.FirstRegistration))
                {
                    vehicleToUpdate.FirstRegistration = DateTime.Parse(inputModel.FirstRegistration);
                }

                if (!string.IsNullOrEmpty(inputModel.Type))
                {
                    vehicleToUpdate.Type = (VehicleType)Enum.Parse(typeof(VehicleType), inputModel.Type);
                }

                this.vehiclesRepo.Update(vehicleToUpdate);
                await this.vehiclesRepo.SaveChangesAsync();
            }
        } // Tested

        public async Task Delete(int id)
        {
            var vehicleToDelete = await this.vehiclesRepo.GetByIdAsync(id);
            if (vehicleToDelete != null)
            {
                vehicleToDelete.IsDeleted = true;
                await this.vehiclesRepo.SaveChangesAsync();
            }
        } // Tested

        public Vehicle GetByVin(string vin)
        {
            return this.vehiclesRepo.All()
                .Include(v => v.Owner)
                .Include(v => v.Policies)
                .FirstOrDefault(v => v.VIN == vin);
        } // Tested

        public string CheckForValidInsurance(Vehicle vehicle)
        {
            return vehicle.Policies
                .FirstOrDefault(p => p.IsValid)
                ?.Id;
        } // Tested

        public bool CheckOwnership(string ownerId, int vehicleId)
            => this.vehiclesRepo.All()
                .Any(v => v.Id == vehicleId
                    && v.OwnerId == ownerId); // Tested
    }
}
