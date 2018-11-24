using SUMI.Models.Enums;
using System;
using System.Collections.Generic;

namespace SUMI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string VIN { get; set; }

        public string NumberPlate { get; set; }

        public DateTime FirstRegistration { get; set; }

        public VehicleType Type { get; set; }

        public bool IsDeleted { get; set; }

        public string OwnerId { get; set; }
        public virtual InsuranceUser Owner { get; set; }

        public virtual ICollection<Policy> Policies { get; set; } = new HashSet<Policy>();
    }
}