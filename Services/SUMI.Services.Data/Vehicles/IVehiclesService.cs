namespace SUMI.Services.Data.Vehicles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SUMI.Data.Models;
    using SUMI.Services.Data.ViewModels;

    public interface IVehiclesService
    {
        Task Create(Vehicle vehicle);

        bool VihicleExists(string vehecleIdentificationNumber);

        IList<Vehicle> GetMyVehicles(string clientId);

        Task<Vehicle> GetByIdAsync(int id);

        IList<Vehicle> GetAll();

        Task Edit(VehicleEditViewModel inputModel);

        Task Delete(int id);

        Task<string> GetNewClientId(Client client);
    }
}
