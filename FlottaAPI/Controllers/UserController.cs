using FlottaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlottaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private FlottaContext _context;

        public UserController(FlottaContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<User> result = _context.Users.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba történt: " + ex.Message);
            }
        }
    }
}
