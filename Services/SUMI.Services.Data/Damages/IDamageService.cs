namespace SUMI.Services.Data.Damages
{
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IDamageService
    {
        Task Add(Damage damage);

        Task<int> RemoveDamage(int id);
    }
}
