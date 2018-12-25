namespace SUMI.Data.Models
{
    using SUMI.Data.Common.Models;
    using SUMI.Data.Models.Enums;

    public class Damage : BaseDeletableModel<int>
    {
        public string PartName { get; set; }

        public Severity Severity { get; set; }

        public decimal EstimatedCost { get; set; }

        public bool IsSettled { get; set; }

        public int ClaimId { get; set; }

        public virtual InsuranceClaim Claim { get; set; }
    }
}
