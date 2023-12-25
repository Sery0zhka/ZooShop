using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using pet_store.purchases.service.Dtos;

namespace pet_store.purchases.service.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient httpClient;

        public CatalogClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<(IReadOnlyCollection<CatalogPetsDto>, IReadOnlyCollection<CatalogCustomerDto>)> GetCatalogPetsDtosAsync()
        {
            var pets = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogPetsDto>>("/pets");
            var customers = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogCustomerDto>>("/pets");

            return (pets, customers);
        }
    }
}