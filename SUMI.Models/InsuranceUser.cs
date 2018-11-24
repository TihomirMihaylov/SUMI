using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SUMI.Models
{
    public class InsuranceUser : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime Birthday { get; set; }

        public string UniversalCitizenNumber { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Policy> Policies { get; set; } = new HashSet<Policy>();

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
    }
}