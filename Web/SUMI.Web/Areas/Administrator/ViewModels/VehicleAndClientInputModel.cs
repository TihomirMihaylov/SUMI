namespace SUMI.Web.Areas.Administrator.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;
    using SUMI.Services.Mapping;
    using SUMI.Web.ViewModels.Vehicles;

    public class VehicleAndClientInputModel : VehicleCreateInputModel, IMapTo<Client>, IHaveCustomMappings
    {
        [Required]
        [Display(Name = "First name")]
        [RegularExpression(@"^[A-Z][a-z]{1,15}$", ErrorMessage = "Invalid format")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [RegularExpression(@"^[A-Z][a-z]{1,15}$", ErrorMessage = "Invalid format")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string Birthday { get; set; }

        [Required]
        [Display(Name = "Universal citizen number")]
        [RegularExpression(@"^[\d]{10}$", ErrorMessage = "Invalid format")]
        public string UniversalCitizenNumber { get; set; }

        public new void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<VehicleAndClientInputModel, Vehicle>()
                .ForMember(x => x.FirstRegistration, x => x.MapFrom(m => DateTime.Parse(m.FirstRegistration)));

            configuration.CreateMap<VehicleAndClientInputModel, Vehicle>()
                .ForMember(x => x.Type, x => x.MapFrom(m => Enum.Parse<VehicleType>(m.Type)));

            configuration.CreateMap<VehicleAndClientInputModel, Vehicle>()
                .ForMember(x => x.CreatedOn, x => x.MapFrom(m => DateTime.UtcNow));

            configuration.CreateMap<VehicleAndClientInputModel, Client>()
                .ForMember(x => x.Birthday, x => x.MapFrom(m => DateTime.Parse(m.Birthday)));

            configuration.CreateMap<VehicleAndClientInputModel, Client>()
                .ForMember(x => x.CreatedOn, x => x.MapFrom(m => DateTime.UtcNow));
        }
    }
}
