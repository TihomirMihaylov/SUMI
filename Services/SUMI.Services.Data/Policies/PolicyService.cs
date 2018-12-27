namespace SUMI.Services.Data.Policies
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;

    public class PolicyService : IPolicyService
    {
        private readonly IDeletableEntityRepository<Policy> policyRepo;

        public PolicyService(IDeletableEntityRepository<Policy> policyRepo)
        {
            this.policyRepo = policyRepo;
        }
    }
}
