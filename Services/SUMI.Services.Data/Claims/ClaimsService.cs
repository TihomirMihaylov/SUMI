namespace SUMI.Services.Data.Claims
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;

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

        public IList<InsuranceClaim> GetMyOpenClaims(string agentId)
            => this.claimRepo.All()
                .Where(c => c.AgentId == agentId && c.Status == ClaimStatus.Open)
                .OrderByDescending(c => c.CreatedOn)
                .ToList();

        public IList<InsuranceClaim> GetMyPendingClaims(string agentId)
            => this.claimRepo.All()
                .Where(c => c.AgentId == agentId && c.Status == ClaimStatus.Pending)
                .OrderByDescending(c => c.CreatedOn)
                .ToList();

        public IList<InsuranceClaim> GetMyResolvedClaims(string agentId)
            => this.claimRepo.All()
                .Where(c => c.AgentId == agentId && c.Status == ClaimStatus.Settled)
                .OrderByDescending(c => c.CreatedOn)
                .ToList();

        public IList<InsuranceClaim> GetAllPendingClaims()
            => this.claimRepo.All()
                .Where(c => c.Status == ClaimStatus.Pending)
                .OrderBy(c => c.CreatedOn)
                .ToList();

        public IList<InsuranceClaim> GetAllResolvedClaims()
            => this.claimRepo.All()
                .Where(c => c.Status == ClaimStatus.Settled)
                .OrderBy(c => c.CreatedOn)
                .ToList();
    }
}
