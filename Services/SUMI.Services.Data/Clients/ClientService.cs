namespace SUMI.Services.Data.Clients
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using SUMI.Data.Common.Repositories;
    using SUMI.Data.Models;

    public class ClientService : IClientService
    {
        private readonly IDeletableEntityRepository<Client> repository;

        public ClientService(IDeletableEntityRepository<Client> repository)
        {
            this.repository = repository;
        }

        public async Task<string> GetClient(string firstName, string lastName, string universalCitizenNumber, DateTime birthday)
        {
            var client = this.repository
                .All()
                .FirstOrDefault(c => c.FirstName == firstName &&
                                c.LastName == lastName &&
                                c.UniversalCitizenNumber == universalCitizenNumber);

            if (client == null)
            {
                client = new Client(firstName, lastName, universalCitizenNumber, birthday);
                this.repository.Add(client);
                await this.repository.SaveChangesAsync();
            }

            return client.Id;
        }
    }
}
