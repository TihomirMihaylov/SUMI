namespace SUMI.Web.ViewModels.Damages
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using SUMI.Data.Models;
    using SUMI.Data.Models.Enums;
    using SUMI.Services.Mapping;

    public class DamagesCreateInputModel : IMapTo<Damage>, IHaveCustomMappings
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Part name may contain only letters")]
        [Display(Name = "Part Name")]
        public string PartName { get; set; }

        [Required]
        public string Severity { get; set; }

        [Required]
        public int ClaimId { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DamagesCreateInputModel, Damage>()
                .ForMember(x => x.Severity, x => x.MapFrom(m => Enum.Parse<Severity>(m.Severity)));
        }
    }
}
