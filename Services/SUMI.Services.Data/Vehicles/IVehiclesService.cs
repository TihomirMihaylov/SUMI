namespace SUMI.Services.Data.Vehicles
{
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IVehiclesService
    {
        Task Create(Vehicle vehicle);

        bool VihicleExists(string vehecleIdentificationNumber);
    }
}
