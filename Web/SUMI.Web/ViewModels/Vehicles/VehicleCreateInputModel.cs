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

    public class VehicleCreateInputModel : IMapTo<Vehicle> // , IHaveCustomMappings
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Invalid format")]
        public string Make { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Invalid format")]
        public string Model { get; set; }

        [Required]
        [RegularExpression(@"^[a-np-zA-NP-Z0-9]{17}$", ErrorMessage = "Invalid format")]
        public string VIN { get; set; }

        [Required]
        [Display(Name = "Number plate")]
        [RegularExpression(@"^[a-zA-Z0-9]{8}$", ErrorMessage = "Invalid format")]
        public string NumberPlate { get; set; }

        [Required]
        [Display(Name = "Date of first registration")]
        [DataType(DataType.Date)]
        public string FirstRegistration { get; set; }

        [Required]
        [Display(Name = "Vehicle type")]
        public string Type { get; set; }

        // public List<SelectListItem> VehicleType { get; } = new List<SelectListItem>
        // {
        //    new SelectListItem { Value = "MX", Text = "Mexico" },
        //    new SelectListItem { Value = "CA", Text = "Canada" },
        //    new SelectListItem { Value = "US", Text = "USA"  },
        // };

        // public void CreateMappings(IMapperConfigurationExpression configuration)
        // {
        //    configuration.CreateMap<VehicleCreateInputModel, Vehicle>().ForMember(x => x.Type, x => x.MapFrom(j => Enum.Parse<VehicleType>(j.Type)));
        // }
    }
}
