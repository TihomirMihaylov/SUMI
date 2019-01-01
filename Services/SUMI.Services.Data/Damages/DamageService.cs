﻿namespace SUMI.Services.Data.Damages
{
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

        public async Task Add(Damage damage)
        {
            this.damageRepo.Add(damage);
            await this.damageRepo.SaveChangesAsync();
        }

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
        }
    }
}
