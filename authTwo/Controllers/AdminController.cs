using authTwo.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace authTwo.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly EntityConnect _context;

        public AdminController(EntityConnect context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.DeletedAt == null)
                    .OrderByDescending(u => u.LastLoginTime)
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        
        [HttpPost("block/{userId}")]
        public async Task<IActionResult> BlockUser(string userId)
        {
            Guid guidId = Guid.Parse(userId);
            var user = await _context.Users.FindAsync(guidId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.IsBlocked = true;
            await _context.SaveChangesAsync();

            return Ok("User blocked successfully.");
        }

        
        [HttpPost("unblock/{userId}")]
        public async Task<IActionResult> UnblockUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.IsBlocked = false;
            await _context.SaveChangesAsync();

            return Ok("User unblocked successfully.");
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.DeletedAt != null)
            {
                return BadRequest("User is already marked as deleted.");
            }

            
            user.DeletedAt = DateTime.UtcNow.ToString();
            await _context.SaveChangesAsync();

            return Ok("User marked as deleted successfully.");
        }
    }
}
