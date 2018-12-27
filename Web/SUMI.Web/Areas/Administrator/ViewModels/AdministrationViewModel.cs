namespace SUMI.Web.Areas.Administrator.ViewModels
{
    using System.Collections.Generic;

    public class AdministrationViewModel
    {
        public IList<UserViewModel> Admins { get; set; } = new List<UserViewModel>();

        public IList<UserViewModel> Agents { get; set; } = new List<UserViewModel>();

        public IList<UserViewModel> Clients { get; set; } = new List<UserViewModel>();
    }
}
