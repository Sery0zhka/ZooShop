using System;
using pet_store.common;

namespace pet_store.catalog.service.Entities
{

    public class Pet : IEntity
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Category { get; set; }

        public int Qty { get; set; }

        public decimal Price { get; set; }
    }
}