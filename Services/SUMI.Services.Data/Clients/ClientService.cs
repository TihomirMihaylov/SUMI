namespace SUMI.Services.Data.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;

    public class ClientService : IClientService
    {
        private readonly IDeletableEntityRepository<Client> clientsRepo;

        public ClientService(IDeletableEntityRepository<Client> clientsRepo)
        {
            this.clientsRepo = clientsRepo;
        }

        public async Task<string> GetClientId(string firstName, string lastName, string universalCitizenNumber, DateTime birthday)
        {
            var client = this.clientsRepo
                .All()
                .FirstOrDefault(c => c.FirstName == firstName &&
                                c.LastName == lastName &&
                                c.UniversalCitizenNumber == universalCitizenNumber);

            if (client == null)
            {
                client = new Client(firstName, lastName, universalCitizenNumber, birthday);
                this.clientsRepo.Add(client);
                await this.clientsRepo.SaveChangesAsync();
                client = this.clientsRepo
                             .All()
                             .FirstOrDefault(c => c.FirstName == firstName &&
                                    c.LastName == lastName &&
                                    c.UniversalCitizenNumber == universalCitizenNumber);
            }

            return client.Id;
        } // Tested

        public IList<Client> GetAll()
        {
            return this.clientsRepo.All()
                .Include(c => c.Policies)
                .Include(c => c.Vehicles)
                .ToList();
        } // Tested
    }
}
