namespace SUMI.Web.ViewModels.Policies
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using SUMI.Data.Models.Enums;

    public class PolicyCreateInputModel
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Invalid format")]
        public string Make { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Invalid format")]
        public string Model { get; set; }

        [Required]
        [RegularExpression(@"^[a-np-zA-NP-Z0-9]{17}$", ErrorMessage = "Invalid format")]
        public string VIN { get; set; }

        [Required]
        [Display(Name = "Number plate")]
        [RegularExpression(@"^[a-zA-Z0-9]{6,8}$", ErrorMessage = "Invalid format")]
        public string NumberPlate { get; set; }

        [Required]
        [Display(Name = "Date of first registration")]
        public string FirstRegistration { get; set; }

        public string OwnerName { get; set; }

        [Required]
        [Display(Name = "Vehicle type")]
        public string Type { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Insurance Sum (EUR)")]
        public decimal InsuranceSum { get; set; }

        [Required]
        public decimal Premium { get; set; }
    }
}
