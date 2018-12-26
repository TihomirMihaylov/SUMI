namespace SUMI.Services.Data.Clients
{
    using System;
    using System.Threading.Tasks;

    public interface IClientService
    {
        Task<string> GetClient(string firstName, string lastName, string universalCitizenNumber, DateTime birthday);
    }
}
