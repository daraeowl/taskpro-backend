using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskpro_api.Models;

namespace taskpro_api.Controllers
{

    [ApiController]
    [Route("api/user")]
    public class AspNetUsersController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AspNetUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();

            return Ok(users);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while fetching user with ID {id}: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, AspNetUsers updatedUser)
        {
            try
            {
                if (id != updatedUser.Id)
                {
                    return BadRequest("ID in URL does not match ID in the request body");
                }

                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                // Update individual properties if provided in the request
                if (updatedUser.Email != null)
                {
                    existingUser.Email = updatedUser.Email;
                }
                // Add similar checks for other properties...

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok($"User with ID {id} updated.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound($"User with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while updating User with ID {id}: {ex.Message}");
            }
        }



        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok($"User with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while deleting user with ID {id}: {ex.Message}");
            }
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


    }

}
