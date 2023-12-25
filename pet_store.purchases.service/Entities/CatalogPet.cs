using System;
using pet_store.common;

namespace pet_store.purchases.service.Entities
{
    public class CatalogPet : IEntity
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public decimal Price { get; set; }

    }
}