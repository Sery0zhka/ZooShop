using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pet_store.common;
using pet_store.purchases.service.Clients;
using pet_store.purchases.service.Dtos;
using pet_store.purchases.service.Entities;

namespace pet_store.purchases.service.Controllers
{
    [ApiController]
    [Route("pets")]
    public class PetsController : ControllerBase
    {
        private readonly IRepository<PurchasesPet> purchasePetsRepository;
        private readonly IRepository<CatalogPet> catalogPetsRepository;
        private readonly IRepository<CatalogCustomer> catalogCustomersRepository;

        public PetsController(IRepository<PurchasesPet> purchasePetsRepository, IRepository<CatalogPet> catalogPetsRepository, IRepository<CatalogCustomer> catalogCustomersRepository)
        {
            this.purchasePetsRepository = purchasePetsRepository;
            this.catalogPetsRepository = catalogPetsRepository;
            this.catalogCustomersRepository = catalogCustomersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchasesPetDto>>> GetAsync()
        {
            var purchasePetEntities = await purchasePetsRepository.GetAllAsync();
            var petIds = purchasePetEntities.Select(pet => pet.CatalogPetId).ToList(); // Преобразуем в список
            var customerIds = purchasePetEntities.Select(customer => customer.CatalogCustomerId).ToList(); // Преобразуем в список
            var catalogPetEntities = await catalogPetsRepository.GetAllAsync(pet => petIds.Contains(pet.Id));
            var catalogCustomerEntities = await catalogCustomersRepository.GetAllAsync(customer => customerIds.Contains(customer.Id));

            var purchasesPetDtos = purchasePetEntities.Select(purchasePet =>
            {
                var catalogPet = catalogPetEntities.SingleOrDefault(catalogPet => catalogPet.Id == purchasePet.CatalogPetId);
                var catalogCustomer = catalogCustomerEntities.SingleOrDefault(catalogCustomer => catalogCustomer.Id == purchasePet.CatalogCustomerId);

                // Проверяем, доступны ли каталоги и подходящие ли они
                if (catalogPet == null || catalogCustomer == null)
                {
                    // Возможно, здесь стоит что-то другое, например, вернуть стандартное значение
                    return purchasePet.AsDto(catalogPet.Type, catalogPet.Price, "-");
                }

                return purchasePet.AsDto(catalogPet.Type, catalogPet.Price, catalogCustomer.CustomerName);
            });

            return Ok(purchasesPetDtos);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantPetsDto grantPetsDto)
        {
            var purchasesPet = await purchasePetsRepository.GetAsync(
                pet => pet.PurchaseId == grantPetsDto.PurchaseId && pet.CatalogPetId == grantPetsDto.CatalogPetId && pet.CatalogCustomerId == grantPetsDto.CatalogCustomerId);

            if (purchasesPet == null)
            {
                purchasesPet = new PurchasesPet
                {
                    CatalogPetId = grantPetsDto.CatalogPetId,
                    PurchaseId = grantPetsDto.PurchaseId,
                    CatalogCustomerId = grantPetsDto.CatalogCustomerId,
                    Qty = grantPetsDto.Qty,
                };

                await purchasePetsRepository.CreateAsync(purchasesPet);
            }
            else
            {
                purchasesPet.Qty = grantPetsDto.Qty;
                await purchasePetsRepository.UpdateAsync(purchasesPet);
            }
            return Ok();
        }

        [HttpPut("{purchaseId}")]
        public async Task<ActionResult> UpdateAsync(Guid purchaseId, [FromBody] GrantPetsDto grantPetsDto)
        {
            var purchasesPet = await purchasePetsRepository.GetAsync(pet => pet.PurchaseId == purchaseId);

            if (purchasesPet != null)
            {
                purchasesPet.PurchaseId = grantPetsDto.PurchaseId;
                purchasesPet.CatalogCustomerId = grantPetsDto.CatalogCustomerId;
                await purchasePetsRepository.UpdateAsync(purchasesPet);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}