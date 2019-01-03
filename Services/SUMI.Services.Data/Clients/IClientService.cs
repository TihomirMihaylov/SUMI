namespace SUMI.Services.Data.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IClientService
    {
        Task<string> GetClient(string firstName, string lastName, string universalCitizenNumber, DateTime birthday);

        IList<Client> GetAll();

        Task<string> GetNewClientId(Client client);
    }
}
