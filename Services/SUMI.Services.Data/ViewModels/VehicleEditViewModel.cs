namespace SUMI.Services.Data.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using SUMI.Data.Models;

    public class VehicleEditViewModel
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string VIN { get; set; }

        public string NumberPlate { get; set; }

        public string FirstRegistration { get; set; }

        public string Type { get; set; }

        public string OwnerName { get; set; }
    }
}
