using System.Threading.Tasks;
using MassTransit;
using pet_store.catalog.contracts;
using pet_store.common;
using pet_store.customers.contracts;
using pet_store.purchases.service.Entities;

namespace pet_store.purchases.service.Consumers
{
    public class CatalogPetDeletedConsumer : IConsumer<CatalogPetDeleted>, IConsumer<CatalogCustomerDeleted>
    {
        private readonly IRepository<CatalogPet> petRepository;
        private readonly IRepository<CatalogCustomer> customerRepository;

        public CatalogPetDeletedConsumer(IRepository<CatalogPet> petRepository, IRepository<CatalogCustomer> customerRepository)
        {
            this.petRepository = petRepository;
            this.customerRepository = customerRepository;
        }

        public async Task Consume(ConsumeContext<CatalogPetDeleted> context)
        {
            var message = context.Message;

            var pet = await petRepository.GetAsync(message.PetId);

            if (pet == null)
            {
                await petRepository.RemoveAsync(message.PetId);
            }
        }

        public async Task Consume(ConsumeContext<CatalogCustomerDeleted> context)
        {
            var message = context.Message;

            var customer = await customerRepository.GetAsync(message.CustomerId);

            if (customer == null)
            {
                await customerRepository.RemoveAsync(message.CustomerId);
            }
        }
    }
}