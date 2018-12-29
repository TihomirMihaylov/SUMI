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
        private readonly IDeletableEntityRepository<Client> clientsRepo;

        public VehiclesService(IDeletableEntityRepository<Vehicle> repository, IDeletableEntityRepository<Client> clientsRepo)
        {
            this.vehiclesRepo = repository;
            this.clientsRepo = clientsRepo;
        }

        public async Task Create(Vehicle vehicle)
        {
            this.vehiclesRepo.Add(vehicle);
            await this.vehiclesRepo.SaveChangesAsync();
        }

        public bool VihicleExists(string vehecleIdentificationNumber)
        {
            return this.vehiclesRepo.All().Any(v => v.VIN == vehecleIdentificationNumber);
        }

        public IList<Vehicle> GetMyVehicles(string clientId)
            => this.vehiclesRepo.All()
                .Where(v => v.OwnerId == clientId)
                .OrderBy(v => v.Make)
                .ThenBy(v => v.Model)
                .ThenBy(v => v.NumberPlate)
                .ToList();

        public Vehicle GetById(int id)
            => this.vehiclesRepo.All()
                .Include(v => v.Owner)
                .FirstOrDefault(v => v.Id == id);

        public IList<Vehicle> GetAll()
            => this.vehiclesRepo.All()
                .OrderBy(v => v.Make)
                .ThenBy(v => v.Model)
                .ThenBy(v => v.NumberPlate)
                .ToList();

        public async Task Edit(VehicleEditViewModel inputModel)
        {
            var vehicleToUpdate = await this.vehiclesRepo.GetByIdAsync(inputModel.Id);
            vehicleToUpdate.Make = inputModel.Make;
            vehicleToUpdate.Model = inputModel.Model;
            vehicleToUpdate.VIN = inputModel.VIN;
            vehicleToUpdate.NumberPlate = inputModel.NumberPlate;
            vehicleToUpdate.FirstRegistration = DateTime.Parse(inputModel.FirstRegistration);
            vehicleToUpdate.Type = (VehicleType)Enum.Parse(typeof(VehicleType), inputModel.Type);

            this.vehiclesRepo.Update(vehicleToUpdate);

            await this.vehiclesRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var vehicleToDelete = await this.vehiclesRepo.GetByIdAsync(id);
            vehicleToDelete.IsDeleted = true;
            await this.vehiclesRepo.SaveChangesAsync();
        }

        public async Task<string> GetNewClientId(Client client)
        {
            var clientFromDb = this.clientsRepo.All()
                .FirstOrDefault(c => c.UniversalCitizenNumber == client.UniversalCitizenNumber);

            if (clientFromDb == null)
            {
                this.clientsRepo.Add(client);
                await this.clientsRepo.SaveChangesAsync();
                clientFromDb = this.clientsRepo.All()
                .FirstOrDefault(c => c.UniversalCitizenNumber == client.UniversalCitizenNumber);
            }

            return clientFromDb.Id;
        }

        public Vehicle GetByVin(string vin)
        {
            return this.vehiclesRepo.All()
                .Include(v => v.Owner)
                .Include(v => v.Policies)
                .FirstOrDefault(v => v.VIN == vin);
        }

        public string CheckForValidInsurance(Vehicle vehicle)
        {
            return vehicle.Policies
                .FirstOrDefault(p => p.IsValid)
                ?.Id;
        }

        private bool ClientExists(string universalCitizenNumber)
        {
            return this.clientsRepo.All().Any(c => c.UniversalCitizenNumber == universalCitizenNumber);
        }
    }
}
