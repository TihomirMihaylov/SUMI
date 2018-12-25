namespace SUMI.Data.Models
{
    using System;
    using System.Collections.Generic;

    using SUMI.Data.Common.Models;
    using SUMI.Data.Models.Enums;

    public class Vehicle : BaseDeletableModel<int>
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public string VIN { get; set; }

        public string NumberPlate { get; set; }

        public DateTime FirstRegistration { get; set; }

        public VehicleType Type { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Policy> Policies { get; set; } = new HashSet<Policy>();
    }
}
