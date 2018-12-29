namespace SUMI.Services.Data.Claims
{
    using System.Threading.Tasks;

    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;

    public class ClaimsService : IClaimsService
    {
        private readonly IDeletableEntityRepository<InsuranceClaim> claimRepo;

        public ClaimsService(IDeletableEntityRepository<InsuranceClaim> claimRepo)
        {
            this.claimRepo = claimRepo;
        }

        public async Task Create(InsuranceClaim claim)
        {
            this.claimRepo.Add(claim);
            await this.claimRepo.SaveChangesAsync();
        }
    }
}
