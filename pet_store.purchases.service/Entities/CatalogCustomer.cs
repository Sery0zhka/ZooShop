using System;
using pet_store.common;

namespace pet_store.purchases.service.Entities
{
    public class CatalogCustomer : IEntity
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; }

    }
}