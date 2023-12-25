using System;

namespace pet_store.purchases.service.Dtos
{
    //Post
    public record GrantPetsDto(Guid PurchaseId, Guid CatalogPetId, Guid CatalogCustomerId, string Type, int Qty, decimal Price, string CustomerName);

    //Get
    public record PurchasesPetDto(Guid PurchaseId,Guid CatalogPetId, Guid CatalogCustomerId, string Type, int Qty, decimal Price, string CustomerName);

    public record CatalogPetsDto(Guid Id, string Type, decimal Price);

    public record CatalogCustomerDto(Guid Id, string CustomerName);

}