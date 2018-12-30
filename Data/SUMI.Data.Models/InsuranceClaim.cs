namespace SUMI.Data.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using SUMI.Data.Common.Models;
    using SUMI.Data.Models.Enums;

    public class InsuranceClaim : BaseDeletableModel<int>
    {
        public string PolicyId { get; set; }

        public virtual Policy Policy { get; set; }

        public string Description { get; set; }

        public decimal TotalCost { get => this.Damages.Sum(d => d.EstimatedCost); }

        public ClaimStatus Status { get; set; }

        public virtual ICollection<Damage> Damages { get; set; } = new HashSet<Damage>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
