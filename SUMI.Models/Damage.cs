using SUMI.Models.Enums;

namespace SUMI.Models
{
    public class Damage
    {
        public int Id { get; set; }

        public string PartName { get; set; }

        public Severity Severity { get; set; }

        public decimal EstimatedCost { get; set; }

        public bool IsSettled { get; set; }

        public int ClaimId { get; set; }
        public virtual Claim Claim { get; set; }
    }
}