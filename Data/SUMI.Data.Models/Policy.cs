﻿namespace SUMI.Data.Models
{
    using System;
    using System.Collections.Generic;

    using SUMI.Data.Common.Models;

    public class Policy : BaseDeletableModel<string>
    {
        public decimal Premium { get; set; }

        public decimal InsuranceSum { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsValid { get; set; }

        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        public string AgentId { get; set; }

        public virtual ApplicationUser Agent { get; set; }

        public int VehicleId { get; set; }

        public virtual Vehicle Vehicle { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<InsuranceClaim> Claims { get; set; } = new HashSet<InsuranceClaim>();
    }
}
