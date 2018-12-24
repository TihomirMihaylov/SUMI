namespace SUMI.Web.ViewModels.Settings
{
    using SUMI.Data.Models;
    using SUMI.Services.Mapping;

    public class SettingViewModel : IMapFrom<Setting>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
