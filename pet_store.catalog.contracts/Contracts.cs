using System;

namespace pet_store.catalog.contracts
{
    public record CatalogPetCreated(Guid PetId, string Type, decimal Price);

    public record CatalogPetUpdated(Guid PetId, string Type, decimal Price);

    public record CatalogPetDeleted(Guid PetId);
}