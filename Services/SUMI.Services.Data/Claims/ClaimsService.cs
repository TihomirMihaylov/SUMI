namespace SUMI.Services.Data.Claims
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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

        public IList<InsuranceClaim> GetMyClaims(string clientId)
            => this.claimRepo.All()
                .Include(c => c.Policy)
                .Where(c => c.Policy.ClientId == clientId)
                .OrderByDescending(c => c.CreatedOn)
                .ToList();

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

        public async Task Delete(int id)
        {
            var claimToDelete = await this.claimRepo.GetByIdAsync(id);
            claimToDelete.IsDeleted = true;
            await this.claimRepo.SaveChangesAsync();
        }

        public InsuranceClaim GetById(int id)
            => this.claimRepo.All()
                .Include(c => c.Comments).ThenInclude(c => c.Author)
                .Include(c => c.Damages)
                .Include(c => c.Policy).ThenInclude(p => p.Vehicle)
                .FirstOrDefault(c => c.Id == id);

        public async Task ChangeStatusToPending(int id)
        {
            var claimToModify = await this.claimRepo.GetByIdAsync(id);
            claimToModify.Status = ClaimStatus.Pending;
            await this.claimRepo.SaveChangesAsync();
        }

        public async Task ChangeStatusToSettled(int id)
        {
            var claimToModify = await this.claimRepo.GetByIdAsync(id);
            claimToModify.Status = ClaimStatus.Settled;
            await this.claimRepo.SaveChangesAsync();
        }
    }
}
