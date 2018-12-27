namespace SUMI.Web.Areas.Administrator.ViewModels
{
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
