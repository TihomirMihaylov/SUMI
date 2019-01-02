namespace SUMI.Services.Data.Damages
{
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IDamageService
    {
        Task AddDamage(Damage damage);

        Task<int> RemoveDamage(int id);

        Task SettleDamage(int id, decimal estimatedCost);

        decimal GetTotalAmountSpentForPolicy(string policyId);
    }
}
