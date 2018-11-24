using System;
using System.Collections.Generic;
using System.Linq;

namespace SUMI.Models
{
    public class Claim
    {
        public int Id { get; set; }

        public string PolicyId { get; set; }
        public virtual Policy Policy { get; set; }

        public string Description { get; set; }

        public decimal TotalCost { get => this.Damages.Sum(d => d.EstimatedCost); }

        public DateTime RegisteredOn { get; set; }

        public virtual ICollection<Damage> Damages { get; set; } = new HashSet<Damage>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}