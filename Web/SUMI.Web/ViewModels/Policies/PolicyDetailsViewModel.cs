namespace SUMI.Web.ViewModels.Policies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class PolicyDetailsViewModel : IMapFrom<Policy>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public decimal Premium { get; set; }

        public decimal InsuranceSum { get; set; }

        public string ExpirationDate { get; set; }

        public bool IsValid { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string ClientBirthday { get; set; }

        public string ClientUniversalCitizenNumber { get; set; }

        public string AgentId { get; set; }

        public ApplicationUser Agent { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleModel { get; set; }

        public string VehicleVIN { get; set; }

        public string VehicleNumberPlate { get; set; }

        public string VehicleFirstRegistration { get; set; }

        public string VehicleType { get; set; }

        public List<string> Comments { get; set; }

        public List<string> Claims { get; set; }

        public decimal TotalSpent { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Policy, PolicyDetailsViewModel>()
                .ForMember(x => x.TotalSpent, x => x.MapFrom(p => p.Claims.Sum(c => c.TotalCost)));

            // TO DO: CHECK THIS LATER
            configuration.CreateMap<Policy, PolicyDetailsViewModel>()
                .ForMember(x => x.Comments, x =>
                    x.MapFrom(p =>
                        p.Comments.Select(c => $"{c.Text} ({c.Author.FirstName} {c.Author.LastName}) --- {c.CreatedOn}")));

            configuration.CreateMap<Policy, PolicyDetailsViewModel>()
                .ForMember(x => x.Claims, x =>
                    x.MapFrom(p =>
                        p.Claims.Select(c => $"Claim N: 01600-SUMI-19-{c.Id}")));
        }
    }
}
