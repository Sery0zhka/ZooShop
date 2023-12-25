using System;
using System.ComponentModel.DataAnnotations;

namespace pet_store.catalog.service.Dtos
{
    public record PetsDto(Guid Id, string Type, string Category, int Qty, decimal Price);

    public record CreatePetsDto([Required] string Type, [Required] string Category, [Range(0, 100)] int Qty, [Range(0, 1000)] decimal Price);

    public record UpdatePetsDto([Required] string Type, [Required] string Category, [Range(0, 100)] int Qty, [Range(0, 1000)] decimal Price);
}