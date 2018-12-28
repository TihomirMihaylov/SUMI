namespace SUMI.Services.Data.Policies
{
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IPolicyService
    {
        bool IsVehicleInsured(int vehicleId);

        decimal GetPremium(decimal insuranceSum, string firstRegistration, string type);

        Task Create(Policy policy);
    }
}
