namespace SUMI.Web.ViewModels.Policies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SUMI.Data.Models.Enums;

    public class PolicyCreateInputModel
    {
        public int VehicleId { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string VIN { get; set; }

        public string NumberPlate { get; set; }

        public DateTime FirstRegistration { get; set; }

        public string OwnerName { get; set; }

        public VehicleType Type { get; set; }

        public string ClientId { get; set; }

        public decimal InsuranceSum { get; set; }

        public decimal Premium { get; set; }
    }
}
