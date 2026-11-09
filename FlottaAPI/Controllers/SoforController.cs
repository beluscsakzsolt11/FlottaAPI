using FlottaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;




namespace FlottaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class soforController : ControllerBase
    {
        private FlottaContext _context;


        public soforController(FlottaContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllSofors()
        {
            try
            {
                List<Sofor> result = _context.Sofors.ToList();


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpGet("GetById/{id}")]
        public IActionResult GetByIdSofors(int id)
        {
            try
            {
                Sofor result = _context.Sofors.FirstOrDefault(s => s.Id == id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }


        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteSofor(int id)
        {
            try
            {
                Sofor result = _context.Sofors.FirstOrDefault(s => s.Id == id);
                if (result != null)
                {
                    _context.Sofors.Remove(result);
                    _context.SaveChanges();
                    return Ok("Sikeres törlés");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting data: {ex.Message}");
            }
        }


        [HttpPost("PostSofor")]
        public IActionResult PostSofor(Sofor sofor)
        {
            try
            {
                _context.Sofors.Add(sofor);
                _context.SaveChanges();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error posting data: {ex.Message}");
            }
        }

        //adott sofőr milyen járművet vezetett?
        [HttpGet("SoforJarmu/{id}")]
        public IActionResult GetSoforJarmu(int id)
        {
            try
            {
                Sofor vane = _context.Sofors.FirstOrDefault(s=>s.Id==id);
                if (vane == null)
                {
                    return NotFound("Hiányzó sofőr");
                }
                var result = _context.Kikuldottjarmus.Where(j=>j.SoforNavigation.Id==id).Select(j=>j.Gepjarmu).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba! : "+ex.Message);
            }
        }
    }
}