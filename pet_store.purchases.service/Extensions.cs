using System;
using pet_store.purchases.service.Dtos;
using pet_store.purchases.service.Entities;

namespace pet_store.purchases.service
{
    public static class Extensions
    {
        public static PurchasesPetDto AsDto(this PurchasesPet pet, string Type, decimal Price, string CustomerName)
        {
            return new PurchasesPetDto(pet.PurchaseId, pet.CatalogPetId, pet.CatalogCustomerId, Type, pet.Qty, Price, CustomerName);
        }
    }
}