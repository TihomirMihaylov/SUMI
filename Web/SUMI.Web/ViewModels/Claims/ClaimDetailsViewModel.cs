namespace SUMI.Web.ViewModels.Claims
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class ClaimDetailsViewModel : IMapFrom<InsuranceClaim>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string PolicyId { get; set; }

        public decimal PolicyPremium { get; set; }

        public decimal PolicyInsuranceSum { get; set; }

        public string PolicyExpirationDate { get; set; }

        public string PolicyVehicleMake { get; set; }

        public string PolicyVehicleModel { get; set; }

        public string PolicyVehicleVIN { get; set; }

        public string PolicyVehicleNumberPlate { get; set; }

        public string PolicyVehicleFirstRegistration { get; set; }

        public string PolicyVehicleType { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Damage> Damages { get; set; }

        public decimal TotalCost { get; set; }

        public decimal TotalSpent { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<InsuranceClaim, ClaimDetailsViewModel>()
                .ForMember(x => x.Damages, x => x.MapFrom(c => c.Damages.Where(d => !d.IsDeleted)));

            // configuration.CreateMap<InsuranceClaim, ClaimDetailsViewModel>()
            //    .ForMember(x => x.Comments, x => x.MapFrom(c => c.Comments.Where(com => !com.IsDeleted)));
        }
    }
}
