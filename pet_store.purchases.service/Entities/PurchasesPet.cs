using System;
using pet_store.common;

namespace pet_store.purchases.service.Entities
{
    public class PurchasesPet : IEntity
    {
        public Guid Id { get; set; }

        public Guid PurchaseId { get; set; }

        public Guid CatalogPetId { get; set; }

        public Guid CatalogCustomerId {get; set;}

        public int Qty { get; set; }

    }
}