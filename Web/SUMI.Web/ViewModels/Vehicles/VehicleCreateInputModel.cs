namespace SUMI.Web.ViewModels.Vehicles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;
    using SUMI.Services.Mapping;

    public class VehicleCreateInputModel : IMapTo<Vehicle>, IHaveCustomMappings
    {
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
        [DataType(DataType.Date)]
        public string FirstRegistration { get; set; }

        [Required]
        [Display(Name = "Vehicle type")]
        public string Type { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<VehicleCreateInputModel, Vehicle>()
                .ForMember(x => x.FirstRegistration, x => x.MapFrom(m => DateTime.Parse(m.FirstRegistration)));

            configuration.CreateMap<VehicleCreateInputModel, Vehicle>()
                .ForMember(x => x.Type, x => x.MapFrom(m => Enum.Parse<VehicleType>(m.Type)));

            configuration.CreateMap<VehicleCreateInputModel, Vehicle>()
                .ForMember(x => x.CreatedOn, x => x.MapFrom(m => DateTime.UtcNow));
        }
    }
}
