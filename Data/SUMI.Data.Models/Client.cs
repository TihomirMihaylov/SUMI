namespace SUMI.Data.Models
{
    using System;
    using System.Collections.Generic;

    using SUMI.Data.Common.Models;

    public class Client : BaseDeletableModel<string>
    {
        public Client(string firstName, string lastName, string universalCitizenNumber, DateTime birthday)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UniversalCitizenNumber = universalCitizenNumber;
            this.Birthday = birthday;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public string UniversalCitizenNumber { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<Policy> Policies { get; set; } = new HashSet<Policy>();

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
    }
}
