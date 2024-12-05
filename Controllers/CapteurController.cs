using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionCapteurs.Data;
using GestionCapteurs.Data.Model;
using Microsoft.Extensions.Caching.Memory;


namespace GestionCapteurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    
    public class CapteurController : ControllerBase
    {
        private readonly AppDbContext _context;
         private readonly IMemoryCache _cache;
        public CapteurController(AppDbContext context,IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        


        // GET: api/Capteur
        [HttpGet, MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<Capteur>>> GetCapteurs()
        {
            
            try
        {
           
            if (!_cache.TryGetValue("Capteurs", out List<Capteur> capteurs))
            {
               
                capteurs = await _context.Capteurs.ToListAsync();

                
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _cache.Set("Capteurs", capteurs, cacheOptions);
            }

            return Ok(capteurs); 
        }
            catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Code = "InternalServerError",
                Message = "Une erreur interne s'est produite.",
                Details = ex.Message
            });
        }
        }

        // GET: api/Capteur/{id}
        [HttpGet("{id}") ,MapToApiVersion("1.0")]
        public async Task<ActionResult<Capteur>> GetCapteur(int id)
        {
        try
        {
            var sensor = await _context.Capteurs.FindAsync(id);
            if (sensor == null)
              return NotFound(new { Code = "CapteurNotFound", Message = "Le capteur demandé est introuvable." });

            return Ok(sensor);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Code = "InternalServerError",
                Message = "Une erreur interne s'est produite.",
                Details = ex.Message
            });
        }

        }

        // POST: api/Capteur
        [HttpPost,MapToApiVersion("1.0")]
        public async Task<ActionResult<Capteur>> CreateCapteur(Capteur capteur)
        {
        try
        {
            if (capteur == null)
                 return BadRequest(new { Code = "InvalidData", Message = "Les données du capteur sont invalides." });

            capteur.CreatedAt = DateTime.UtcNow;
            _context.Capteurs.Add(capteur);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCapteur), new { id = capteur.Id }, capteur);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Code = "InternalServerError", Message = "Une erreur interne s'est produite.",  Details = ex.Message
            });
        }

        }

        // PUT: api/Capteur/{id}
        [HttpPut("{id}"),MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCapteur(int id, Capteur capteur)
        {
            if (capteur == null || id != capteur.Id)
                 return BadRequest(new { Code = "InvalidData", Message = "Les données du capteur sont invalides." });

            var existingCapteur = await _context.Capteurs.FindAsync(id);
            if (existingCapteur == null)
                 return BadRequest(new { Code = "InvalidData", Message = "Les données du capteur sont invalides." });
            existingCapteur.Name = capteur.Name;
            existingCapteur.Type = capteur.Type;
            existingCapteur.CreatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "Un problème de concurrence s'est produit." });
            }

            return NoContent();
        }

        // DELETE: api/Capteur/{id}
        [HttpDelete("{id}"),MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteCapteur(int id)
        {
            var capteur = await _context.Capteurs.FindAsync(id);
            if (capteur == null)
                return BadRequest(new { Code = "InvalidData", Message = "Les données du capteur sont invalides." });

            _context.Capteurs.Remove(capteur);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
