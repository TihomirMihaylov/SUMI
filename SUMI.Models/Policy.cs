using System;
using System.Collections.Generic;

namespace SUMI.Models
{
    public class Policy
    {
        public string Id { get; set; }

        public decimal Premium { get; set; }

        public decimal InsuranceSum { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsValid { get; set; }

        public string ClientId { get; set; }
        public virtual InsuranceUser Client { get; set; }

        public string AgentId { get; set; }
        public virtual InsuranceUser Agent { get; set; }

        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Claim> Claims { get; set; } = new HashSet<Claim>();
    }
}