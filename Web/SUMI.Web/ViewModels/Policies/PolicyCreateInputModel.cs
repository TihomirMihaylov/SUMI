namespace SUMI.Web.ViewModels.Policies
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class PolicyCreateInputModel : IMapTo<Policy>, IHaveCustomMappings
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

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //configuration.CreateMap<PolicyCreateInputModel, Policy>()
            //    .ForMember(x => x.CreatedOn, x => x.MapFrom(m => new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day)));

            //configuration.CreateMap<PolicyCreateInputModel, Policy>()
            //    .ForMember(x => x.ExpirationDate, x => x.MapFrom(m => new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddYears(1)));

            //configuration.CreateMap<PolicyCreateInputModel, Policy>()
            //    .ForMember(x => x.IsValid, x => x.MapFrom(m => true));
        }
    }
}
