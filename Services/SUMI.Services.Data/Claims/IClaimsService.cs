﻿namespace SUMI.Services.Data.Claims
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IClaimsService
    {
        Task Create(InsuranceClaim claim);

        IList<InsuranceClaim> GetMyOpenClaims(string agentId);

        IList<InsuranceClaim> GetMyPendingClaims(string agentId);

        IList<InsuranceClaim> GetMyResolvedClaims(string agentId);

        IList<InsuranceClaim> GetAllPendingClaims();

        IList<InsuranceClaim> GetAllResolvedClaims();
    }
}
