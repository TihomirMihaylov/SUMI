namespace SUMI.Services.Data.Policies
{
    using System;
    using System.Linq;

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
        }

        public decimal GetPremium(decimal insuranceSum, string firstRegistration, string type)
        {
            DateTime registrationDate = DateTime.Parse(firstRegistration);
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

            VehicleType vehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), type);
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
        }
    }
}
