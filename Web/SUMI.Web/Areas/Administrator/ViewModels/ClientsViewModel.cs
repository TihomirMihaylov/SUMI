namespace SUMI.Web.Areas.Administrator.ViewModels
{
    using AutoMapper;
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class ClientsViewModel : IMapFrom<Client>, IHaveCustomMappings
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int PoliciesCount { get; set; }

        public int VehiclesCount { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Client, ClientsViewModel>()
                .ForMember(x => x.PoliciesCount, x => x.MapFrom(c => c.Policies.Count));

            configuration.CreateMap<Client, ClientsViewModel>()
                .ForMember(x => x.VehiclesCount, x => x.MapFrom(c => c.Vehicles.Count));
        }
    }
}
