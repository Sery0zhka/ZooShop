using pet_store.catalog.service.Dtos;
using pet_store.catalog.service.Entities;

namespace pet_store.catalog.service
{
    public static class Extensions
    {
        public static PetsDto AsDto(this Pet pet)
        {
            return new PetsDto(pet.Id, pet.Type, pet.Category, pet.Qty, pet.Price);
        }
    }
}