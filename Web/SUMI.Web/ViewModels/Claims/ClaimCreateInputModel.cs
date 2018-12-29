namespace SUMI.Web.ViewModels.Claims
{
    using System.ComponentModel.DataAnnotations;

    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class ClaimCreateInputModel : IMapTo<InsuranceClaim>
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        [RegularExpression(@"^[a-np-zA-NP-Z0-9]{17}$", ErrorMessage = "Invalid format")]
        public string VIN { get; set; }

        [Required]
        public string PolicyId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
