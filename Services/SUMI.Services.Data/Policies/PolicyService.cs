namespace SUMI.Services.Data.Policies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SUMI.Common;
    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;

    public class PolicyService : IPolicyService
    {
        private readonly IDeletableEntityRepository<Policy> policyRepo;

        public PolicyService(IDeletableEntityRepository<Policy> policyRepo)
        {
            this.policyRepo = policyRepo;
        }

        public bool IsVehicleInsured(int vehicleId)
        {
            return this.policyRepo.All().Any(p => p.VehicleId == vehicleId && p.IsValid == true);
        } // Tested

        public decimal GetPremium(decimal insuranceSum, string firstRegistration, string type)
        {
            if (insuranceSum <= 0)
            {
                return default;
            }

            DateTime registrationDate = DateTime.Now;
            try
            {
                registrationDate = DateTime.Parse(firstRegistration);
            }
            catch
            {
                return 0;
            }

            int days = (DateTime.Today - registrationDate).Days;
            decimal years = days / 365.25m;
            decimal basePremium = 0;
            switch (years)
            {
                case decimal n when n <= 3:
                    basePremium = GlobalConstants.BasePremiumUpTo3Years;
                    break;
                case decimal n when n > 3 && n <= 10:
                    basePremium = GlobalConstants.BasePremiumBetween3And10Years;
                    break;
                default:
                    basePremium = GlobalConstants.BasePremiumMoreThan10Years;
                    break;
            }

            VehicleType vehicleType = 0;
            try
            {
                vehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), type);
            }
            catch
            {
                return 0;
            }

            decimal multiplier = 1;
            if (vehicleType == VehicleType.Truck)
            {
                multiplier = GlobalConstants.MultiplierForTrucks;
            }
            else if (vehicleType == VehicleType.Motorcycle)
            {
                multiplier = GlobalConstants.MultiplierForMotorcycles;
            }

            return insuranceSum * basePremium * multiplier;
        } // Tested

        public async Task Create(Policy policy)
        {
            this.policyRepo.Add(policy);
            await this.policyRepo.SaveChangesAsync();
        } // Tested!

        public IList<Policy> GetMyPolicies(string id)
            => this.policyRepo.AllWithDeleted()
                .Where(p => p.ClientId == id)
                .Include(p => p.Vehicle)
                .OrderByDescending(p => p.CreatedOn)
                .ToList(); // Tested

        public IList<Policy> GetMyPoliciesIssued(string id)
            => this.policyRepo.AllWithDeleted()
                .Where(p => p.AgentId == id)
                .Include(p => p.Vehicle)
                .OrderByDescending(p => p.CreatedOn)
                .ToList(); // Tested

        public IList<Policy> GetAllActivePolicies()
            => this.policyRepo.AllWithDeleted()
                .Where(p => p.IsValid)
                .Include(p => p.Vehicle)
                .OrderByDescending(p => p.CreatedOn)
                .ToList(); // Tested

        public IList<Policy> GetAllExpiredPolicies()
            => this.policyRepo.AllWithDeleted()
                .Where(p => !p.IsValid)
                .Include(p => p.Vehicle)
                .OrderByDescending(p => p.CreatedOn)
                .ToList(); // Tested

        public Policy GetById(string id)
            => this.policyRepo.AllWithDeleted()
                .Include(p => p.Agent)
                .Include(p => p.Claims).ThenInclude(c => c.Damages)
                .Include(p => p.Client)
                .Include(p => p.Comments).ThenInclude(c => c.Author)
                .Include(p => p.Vehicle)
                .FirstOrDefault(p => p.Id == id); // Tested!

        public async Task<bool> TerminatePolicy(string id)
        {
            var policy = await this.policyRepo.GetByIdAsync(id);
            if (policy == null)
            {
                return false;
            }

            if (policy.IsValid == false)
            {
                return false;
            }

            policy.IsValid = false;
            await this.policyRepo.SaveChangesAsync();
            return true;
        } // Tested
    }
}
