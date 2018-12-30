namespace SUMI.Web.ViewModels.Claims
{
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class ClaimViewModel : IMapFrom<InsuranceClaim>
    {
        public int Id { get; set; }

        public string CreatedOn { get; set; }
    }
}
