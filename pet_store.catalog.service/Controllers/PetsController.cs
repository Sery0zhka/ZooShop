using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using pet_store.catalog.service.Dtos;
using pet_store.catalog.service.Entities;
using pet_store.common;
using pet_store.catalog.contracts;

namespace pet_store.catalog.service.Controllers
{
    //https://localhost:5001/pets
    [ApiController]
    [Route("pets")]
    public class PetsController : ControllerBase
    {

        private readonly IRepository<Pet> petsRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public PetsController(IRepository<Pet> petsRepository, IPublishEndpoint publishEndpoint)
        {
            this.petsRepository = petsRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetsDto>>> GetAsync()
        {
            var pets = (await petsRepository.GetAllAsync())
                       .Select(pets => pets.AsDto());

            return Ok(pets);
        }

        // Get /pets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PetsDto>> GetByIdAsync(Guid id)
        {
            var pet = await petsRepository.GetAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            return pet.AsDto();
        }

        // Post /pets
        [HttpPost]
        public async Task<ActionResult<PetsDto>> PostAsync(CreatePetsDto createPetsDto)
        {
            var pet = new Pet
            {
                Type = createPetsDto.Type,
                Category = createPetsDto.Category,
                Qty = createPetsDto.Qty,
                Price = createPetsDto.Price
            };

            await petsRepository.CreateAsync(pet);

            await publishEndpoint.Publish(new CatalogPetCreated(pet.Id, pet.Type, pet.Price));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = pet.Id }, pet);

        }

        // Put /pets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdatePetsDto updatePetsDto)
        {
            var existingPet = await petsRepository.GetAsync(id);

            if (existingPet == null)
            {
                return NotFound();
            }
            existingPet.Type = updatePetsDto.Type;
            existingPet.Category = updatePetsDto.Category;
            existingPet.Qty = updatePetsDto.Qty;
            existingPet.Price = updatePetsDto.Price;

            await petsRepository.UpdateAsync(existingPet);

            await publishEndpoint.Publish(new CatalogPetUpdated(existingPet.Id, existingPet.Type, existingPet.Price));

            return NoContent();
        }

        // Delete /pets/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var pet = await petsRepository.GetAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            await petsRepository.RemoveAsync(pet.Id);

            await publishEndpoint.Publish(new CatalogPetDeleted(id));

            return NoContent();
        }
    }
}