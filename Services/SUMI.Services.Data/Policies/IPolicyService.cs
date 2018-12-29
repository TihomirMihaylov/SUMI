namespace SUMI.Services.Data.Policies
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IPolicyService
    {
        bool IsVehicleInsured(int vehicleId);

        decimal GetPremium(decimal insuranceSum, string firstRegistration, string type);

        Task Create(Policy policy);

        IList<Policy> GetMyPolicies(string id);

        IList<Policy> GetMyPoliciesIssued(string id);

        IList<Policy> GetAllActivePolicies();

        IList<Policy> GetAllExpiredPolicies();

        Policy GetById(string id);

        Task<bool> TerminatePolicy(string id);
    }
}
