using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlottaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FlottaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GepjarmuController : ControllerBase
    {
        private FlottaContext _context;
        public GepjarmuController(FlottaContext context)
        {
            _context = context;
        }
        //getall
        [HttpGet("GetAllGepjarmu")]
        public IActionResult GetAllGepjarmu()
        {
            try
            {
                List<Gepjarmu> result = _context.Gepjarmus.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba történt: " + ex.Message);
            }
        }
        //getbyrendszam
        [HttpGet("GetByRendszam/{rendszam}")]
        public IActionResult GetByRendszam(string rendszam)
        {
            try
            {
                var result = _context.Gepjarmus.FirstOrDefault(g => g.Rendszam == rendszam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba történt: " + ex.Message);
            }
        }

        //post
        [HttpPost("UjGepjarmu")]
        public IActionResult UjGepjarmu(Gepjarmu gepjarmu)
        {
            try
            {
                if (gepjarmu == null)
                {
                    return BadRequest("Hiányzó adatok");
                }
                _context.Gepjarmus.Add(gepjarmu);
                _context.SaveChanges();
                return Ok("Sikeres rögzítés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba történt: " + ex.Message);
            }
        }
        //ki vezette?
        [HttpGet("GetByKiVezette")]
        public IActionResult KiVezette(int id)
        {
            try
            {
                if(_context.Gepjarmus.FirstOrDefault(g=>g.Id==id)==null)
                {
                    return NotFound("Hiányzó gépjármű");
                }
                List<Sofor> result = _context.Kikuldottjarmus.Include(k => k.SoforNavigation)
                    .Include(k => k.Gepjarmu)
                    .Where(k => k.GepjarmuId == id)
                    .Select(k => k.SoforNavigation).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba: " + ex.Message);
            }

        }

        [HttpDelete("TorlesGepjarmu/{id}")]
        public IActionResult TorlesGepjarmu(int id)
        {
            try
            {
                if (_context.Gepjarmus.FirstOrDefault(g => g.Id == id) == null)
                {
                    return NotFound("Hiányzó gépjármű");
                }
                _context.ChangeTracker.Clear();
                _context.Gepjarmus.Remove(new Gepjarmu { Id=id});
                _context.SaveChanges();
                return Ok("Sikeres törlés!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba történt: " + ex.Message);
            }
        }
    }
}