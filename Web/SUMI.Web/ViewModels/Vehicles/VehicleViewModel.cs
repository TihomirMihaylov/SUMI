namespace SUMI.Web.ViewModels.Vehicles
{
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class VehicleViewModel : IMapFrom<Vehicle>
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string NumberPlate { get; set; }
    }
}
