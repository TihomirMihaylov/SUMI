namespace SUMI.Services.Data.Damages
{
    using System.Linq;
    using System.Threading.Tasks;

    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;

    public class DamageService : IDamageService
    {
        private readonly IDeletableEntityRepository<Damage> damageRepo;

        public DamageService(IDeletableEntityRepository<Damage> damageRepo)
        {
            this.damageRepo = damageRepo;
        }

        public async Task AddDamage(Damage damage)
        {
            this.damageRepo.Add(damage);
            await this.damageRepo.SaveChangesAsync();
        } // Tested

        public async Task<int> RemoveDamage(int id)
        {
            var damage = await this.damageRepo.GetByIdAsync(id);
            if (damage != null)
            {
                this.damageRepo.Delete(damage);
                await this.damageRepo.SaveChangesAsync();
                return damage.ClaimId;
            }

            return -1;
        } // Tested

        public async Task SettleDamage(int id, decimal estimatedCost)
        {
            var damage = await this.damageRepo.GetByIdAsync(id);
            if (damage != null)
            {
                damage.EstimatedCost = estimatedCost;
                damage.IsSettled = true;
                await this.damageRepo.SaveChangesAsync();
            }
        } // Tested

        public decimal GetTotalAmountSpentForPolicy(string policyId)
            => this.damageRepo.All()
                .Where(d => d.PolicyId == policyId)
                .Sum(d => d.EstimatedCost); // Tested
    }
}
