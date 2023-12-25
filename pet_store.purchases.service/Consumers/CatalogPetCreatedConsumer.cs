using System.Threading.Tasks;
using MassTransit;
using pet_store.catalog.contracts;
using pet_store.common;
using pet_store.customers.contracts;
using pet_store.purchases.service.Entities;

namespace pet_store.purchases.service.Consumers
{
    public class CatalogPetCreatedConsumer : IConsumer<CatalogPetCreated>, IConsumer<CatalogCustomerCreated>
    {
        private readonly IRepository<CatalogPet> petRepository;
        private readonly IRepository<CatalogCustomer> customerRepository;

        public CatalogPetCreatedConsumer(IRepository<CatalogPet> petRepository, IRepository<CatalogCustomer> customerRepository)
        {
            this.petRepository = petRepository;
            this.customerRepository = customerRepository;
        }


        public async Task Consume(ConsumeContext<CatalogPetCreated> context)
        {
            var message = context.Message;

            var pet = await petRepository.GetAsync(message.PetId);

            if (pet != null)
            {
                return;
            }

            pet = new CatalogPet
            {
                Id = message.PetId,
                Type = message.Type,
                Price = message.Price
            };

            await petRepository.CreateAsync(pet);
        }

        public async Task Consume(ConsumeContext<CatalogCustomerCreated> context)
        {
            var message = context.Message;

            var customer = await customerRepository.GetAsync(message.CustomerId);

            if (customer != null)
            {
                return;
            }

            customer = new CatalogCustomer
            {
                Id = message.CustomerId,
                CustomerName = message.CustomerName
            };

            await customerRepository.CreateAsync(customer);
        }
    }
}