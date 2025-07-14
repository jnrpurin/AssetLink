using InventoryTracker.Dtos;
using InventoryTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
        {
            _logger.LogInformation("GET ALL Users request received.");
            try
            {
                var users = await _service.GetAllAsync();
                _logger.LogInformation("Returning {Count} users.", users.Count());
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            _logger.LogInformation("GET User by ID: {Id} request received.", id);
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user is null)
                {
                    _logger.LogWarning("User with ID: {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("User with ID: {Id} found successfully.", id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with ID: {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Create(UserCreateDto dto)
        {
            _logger.LogInformation("POST request to create user received. Email: {Email}", dto.EmailAddress);
            try
            {
                var createdUser = await _service.CreateAsync(dto);
                _logger.LogInformation("User created successfully with ID: {Id}.", createdUser.Id);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with Email: {Email}.", dto.EmailAddress);
                return StatusCode(500, "Internal server error while creating user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserCreateDto dto) // Usando CreateDto para update
        {
            _logger.LogInformation("PUT request to update user ID: {Id} received.", id);
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (!updated)
                {
                    _logger.LogWarning("Update failed: User with ID: {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("User with ID: {Id} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID: {Id}.", id);
                return StatusCode(500, "Internal server error while updating user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE request for user ID: {Id} received.", id);
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Delete failed: User with ID: {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("User with ID: {Id} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {Id}.", id);
                return StatusCode(500, "Internal server error while deleting user.");
            }
        }
    }
}
