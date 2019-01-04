namespace SUMI.Services.Data.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SUMI.Data.Models;

    public interface IClientService
    {
        Task<string> GetClientId(string firstName, string lastName, string universalCitizenNumber, DateTime birthday);

        IList<Client> GetAll();
    }
}
