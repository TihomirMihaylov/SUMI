﻿namespace SUMI.Services.Data.Claims
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IClaimService
    {
        Task Create(InsuranceClaim claim);

        IList<InsuranceClaim> GetMyClaims(string clientId);

        IList<InsuranceClaim> GetMyOpenClaims(string agentId);

        IList<InsuranceClaim> GetMyPendingClaims(string agentId);

        IList<InsuranceClaim> GetMyResolvedClaims(string agentId);

        IList<InsuranceClaim> GetAllPendingClaims();

        IList<InsuranceClaim> GetAllResolvedClaims();

        Task Delete(int id);

        InsuranceClaim GetById(int id);

        Task ChangeStatusToPending(int id);

        Task ChangeStatusToSettled(int id);

        IList<InsuranceClaim> GetAllUnsettledByPolicyId(string policyId);

        bool CheckOwnership(string ownerId, int claimId);

        bool CheckCreator(string userId, int claimId);
    }
}
