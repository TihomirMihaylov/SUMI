namespace SUMI.Services.Data.Vehicles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SUMI.Data.Common;
    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;
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

        public IList<VehicleViewModel> GetMyVehicles(string clientId)
        {
            return this.vehiclesRepo
                .All()
                .Where(v => v.OwnerId == clientId)
                .Select(v => new VehicleViewModel
                {
                   Id = v.Id,
                   Make = v.Make,
                   Model = v.Model,
                }).ToList();
        }

        public async Task<Vehicle> GetById(int id)
        {
            // var result = this.vehiclesRepo.All()
            //    .Join(
            //        this.clientsRepo.All(),
            //        v => v.OwnerId,
            //        c => c.Id,
            //        (v, c) => new { Vehicle = v, Client = c })
            //    .FirstOrDefault(x => x.Vehicle.OwnerId == x.Client.Id);
            return await this.vehiclesRepo.GetByIdAsync(id);
        }
    }
}
