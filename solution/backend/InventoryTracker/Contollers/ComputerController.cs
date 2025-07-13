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
        private readonly ILogger<ComputersController> _logger;

        public ComputersController(IComputerService service, ILogger<ComputersController> logger)
        {
            _service = service;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComputerReadDto>>> GetAll()
        {
            _logger.LogInformation("GET ALL Computers request received."); 
            var computers = await _service.GetAllAsync();

            _logger.LogInformation("Returning {Count} computers.", computers.Count()); 
            return Ok(computers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComputerReadDto>> GetById(int id)
        {
            _logger.LogInformation("GET Computer by ID: {Id} request received.", id); 
            
            var computer = await _service.GetByIdAsync(id);
            if (computer is null)
            {
                _logger.LogWarning("Computer with ID: {Id} not found.", id); 
                return NotFound();
            }

            _logger.LogInformation("Computer with ID: {Id} found successfully.", id); 
            return Ok(computer);
        }

        [HttpPost]
        public async Task<ActionResult<ComputerReadDto>> Create(ComputerCreateDto dto)
        {
            _logger.LogInformation("POST request to create computer received. SerialNumber: {SerialNumber}", dto.SerialNumber); 
            var createdComputer = await _service.CreateAsync(dto);

            _logger.LogInformation("Computer created successfully with ID: {Id}.", createdComputer.Id); 
            return CreatedAtAction(nameof(GetById), new { id = createdComputer.Id }, createdComputer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ComputerUpdateDto dto)
        {
            _logger.LogInformation("PUT request to update computer ID: {Id} received.", id); 
            
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
            {
                _logger.LogWarning("Update failed: Computer with ID: {Id} not found.", id); 
                return NotFound();
            }

            _logger.LogInformation("Computer with ID: {Id} updated successfully.", id); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE request for computer ID: {Id} received.", id); 

            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Delete failed: Computer with ID: {Id} not found.", id); 
                return NotFound();
            }

            _logger.LogInformation("Computer with ID: {Id} deleted successfully.", id); 
            return NoContent();
        }
        
        [HttpPost("assign")] 
        public async Task<IActionResult> AssignComputer([FromBody] ComputerAssignUserDto dto)
        {
            _logger.LogInformation("POST request to assign computer ID: {ComputerId} to User ID: {UserId} received.", dto.ComputerId, dto.UserId);
            var success = await _service.AssignComputerToUserAsync(dto);

            if (!success)
            {
                _logger.LogWarning("Assignment failed for Computer ID: {ComputerId} to User ID: {UserId}.", dto.ComputerId, dto.UserId);
                return BadRequest("Could not assign computer. Check computer or user ID.");
            }

            _logger.LogInformation("Computer ID: {ComputerId} successfully assigned to User ID: {UserId}.", dto.ComputerId, dto.UserId);
            return Ok("Computer assigned successfully.");
        }
    }
}
