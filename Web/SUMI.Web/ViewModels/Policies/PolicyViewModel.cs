namespace SUMI.Web.ViewModels.Policies
{
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class PolicyViewModel : IMapFrom<Policy>
    {
        public string Id { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleModel { get; set; }

        public string VehicleNumberPlate { get; set; }

        public string ExpirationDate { get; set; }
    }
}
