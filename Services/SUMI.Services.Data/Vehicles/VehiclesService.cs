namespace SUMI.Services.Data.Vehicles
{
    using System.Linq;
    using System.Threading.Tasks;

    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;

    public class VehiclesService : IVehiclesService
    {
        private readonly IDeletableEntityRepository<Vehicle> repository;

        public VehiclesService(IDeletableEntityRepository<Vehicle> repository)
        {
            this.repository = repository;
        }

        public async Task Create(Vehicle vehicle)
        {
            this.repository.Add(vehicle);
            await this.repository.SaveChangesAsync();
        }

        public bool VihicleExists(string vehecleIdentificationNumber)
        {
            return this.repository.All().Any(v => v.VIN == vehecleIdentificationNumber);
        }
    }
}
