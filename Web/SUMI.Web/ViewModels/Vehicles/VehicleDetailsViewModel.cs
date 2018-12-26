namespace SUMI.Web.ViewModels.Vehicles
{
    using AutoMapper;
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class VehicleDetailsViewModel : IMapTo<Vehicle>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string VIN { get; set; }

        public string NumberPlate { get; set; }

        public string FirstRegistration { get; set; }

        public string Type { get; set; }

        public string OwnerName { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Vehicle, VehicleDetailsViewModel>()
                .ForMember(x => x.FirstRegistration, x => x.MapFrom(v => v.FirstRegistration.ToShortDateString()));

            configuration.CreateMap<Vehicle, VehicleDetailsViewModel>()
                .ForMember(x => x.Type, x => x.MapFrom(v => v.Type.ToString()));

            configuration.CreateMap<Vehicle, VehicleDetailsViewModel>()
                .ForMember(x => x.OwnerName, x => x.MapFrom(v => $"{v.Owner.FirstName} {v.Owner.LastName}"));
        }
    }
}
