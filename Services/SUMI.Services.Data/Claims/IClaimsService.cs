namespace SUMI.Services.Data.Claims
{
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IClaimsService
    {
        Task Create(InsuranceClaim claim);
    }
}
