namespace SUMI.Services.Data.Claims
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;

    public class ClaimService : IClaimService
    {
        private readonly IDeletableEntityRepository<InsuranceClaim> claimRepo;

        public ClaimService(IDeletableEntityRepository<InsuranceClaim> claimRepo)
        {
            this.claimRepo = claimRepo;
        }

        public async Task Create(InsuranceClaim claim)
        {
            this.claimRepo.Add(claim);
            await this.claimRepo.SaveChangesAsync();
        } // Tested

        public IList<InsuranceClaim> GetMyClaims(string clientId)
            => this.claimRepo.All()
                .Include(c => c.Policy)
                .Where(c => c.Policy.ClientId == clientId)
                .OrderByDescending(c => c.CreatedOn)
                .ToList(); // Tested

        public IList<InsuranceClaim> GetMyOpenClaims(string agentId)
            => this.claimRepo.All()
                .Where(c => c.AgentId == agentId && c.Status == ClaimStatus.Open)
                .OrderByDescending(c => c.CreatedOn)
                .ToList(); // Tested

        public IList<InsuranceClaim> GetMyPendingClaims(string agentId)
            => this.claimRepo.All()
                .Where(c => c.AgentId == agentId && c.Status == ClaimStatus.Pending)
                .OrderByDescending(c => c.CreatedOn)
                .ToList(); // Tested

        public IList<InsuranceClaim> GetMyResolvedClaims(string agentId)
            => this.claimRepo.All()
                .Where(c => c.AgentId == agentId && c.Status == ClaimStatus.Settled)
                .OrderByDescending(c => c.CreatedOn)
                .ToList(); // Tested

        public IList<InsuranceClaim> GetAllPendingClaims()
            => this.claimRepo.All()
                .Where(c => c.Status == ClaimStatus.Pending)
                .OrderBy(c => c.CreatedOn)
                .ToList(); // Tested

        public IList<InsuranceClaim> GetAllResolvedClaims()
            => this.claimRepo.All()
                .Where(c => c.Status == ClaimStatus.Settled)
                .OrderByDescending(c => c.CreatedOn)
                .ToList(); // Tested

        public async Task Delete(int id)
        {
            var claimToDelete = await this.claimRepo.GetByIdAsync(id);
            if (claimToDelete != null)
            {
                claimToDelete.IsDeleted = true;
                await this.claimRepo.SaveChangesAsync();
            }
        } // Tested

        public InsuranceClaim GetById(int id)
            => this.claimRepo.All()
                .Include(c => c.Comments).ThenInclude(c => c.Author)
                .Include(c => c.Damages)
                .Include(c => c.Policy).ThenInclude(p => p.Vehicle)
                .Include(c => c.Policy).ThenInclude(p => p.Claims)
                .FirstOrDefault(c => c.Id == id); // Tested

        public async Task ChangeStatusToPending(int id)
        {
            var claimToModify = await this.claimRepo.GetByIdAsync(id);
            if (claimToModify != null)
            {
                claimToModify.Status = ClaimStatus.Pending;
                await this.claimRepo.SaveChangesAsync();
            }
        } // Tested

        public async Task ChangeStatusToSettled(int id)
        {
            var claimToModify = await this.claimRepo.GetByIdAsync(id);
            if (claimToModify != null)
            {
                claimToModify.Status = ClaimStatus.Settled;
                await this.claimRepo.SaveChangesAsync();
            }
        } // Tested

        public IList<InsuranceClaim> GetAllUnsettledByPolicyId(string policyId)
            => this.claimRepo.All()
               .Where(c => c.PolicyId == policyId && c.Status != ClaimStatus.Settled).ToList(); // Tested
    }
}
