namespace SUMI.Web.Areas.Administrator.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class DamageSettlementInputModel
    {
        [Required]
        public int DamageId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue)]
        public decimal EstimatedCost { get; set; }
    }
}
