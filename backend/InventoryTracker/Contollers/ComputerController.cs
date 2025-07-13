using InventoryTracker.Dtos;
using InventoryTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComputersController : ControllerBase
    {
        private readonly IComputerService _service;

        public ComputersController(IComputerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComputerReadDto>>> GetAll()
        {
            var computers = await _service.GetAllAsync();
            return Ok(computers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComputerReadDto>> GetById(int id)
        {
            var computer = await _service.GetByIdAsync(id);
            if (computer is null)
                return NotFound();

            return Ok(computer);
        }

        [HttpPost]
        public async Task<ActionResult<ComputerReadDto>> Create(ComputerCreateDto dto)
        {
            var createdComputer = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdComputer.Id }, createdComputer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ComputerUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
