namespace SUMI.Services.Data.Policies
{
    public interface IPolicyService
    {
        bool IsVehicleInsured(int vehicleId);

        decimal GetPremium(decimal insuranceSum, string firstRegistration, string type);
    }
}
