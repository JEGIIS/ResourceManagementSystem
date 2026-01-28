using CompanyResources.API.Data;
using CompanyResources.API.Hubs;
using CompanyResources.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CompanyResources.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ResourceHub> _hubContext;

        public ResourcesController(AppDbContext context, IHubContext<ResourceHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Resource>>> GetAll()
        {
            return await _context.Resources.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Resource>> Create(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            // Powiadom wszystkich klientów o nowym zasobie
            await _hubContext.Clients.All.SendAsync("ReceiveResourceUpdate");

            return CreatedAtAction(nameof(GetAll), new { id = resource.Id }, resource);
        }

        // Endpoint do szybkiego resetowania bazy (dla testów)
        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            if (!_context.Resources.Any())
            {
                _context.Resources.Add(new Resource { Name = "Laptop Dell", Type = "Sprzęt", IsAvailable = true });
                _context.Resources.Add(new Resource { Name = "Sala Konferencyjna A", Type = "Sala", IsAvailable = false });
                await _context.SaveChangesAsync();
            }
            return Ok("Dane dodane");
        }
        // EDYCJA (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Resource resource)
        {
            if (id != resource.Id) return BadRequest("Błąd ID");

            var existingResource = await _context.Resources.FindAsync(id);
            if (existingResource == null) return NotFound();

            // Aktualizacja pól
            existingResource.Name = resource.Name;
            existingResource.Type = resource.Type;
            existingResource.IsAvailable = resource.IsAvailable; // Dodajemy też możliwość zmiany statusu

            await _context.SaveChangesAsync();

            // Powiadom wszystkich o zmianie
            await _hubContext.Clients.All.SendAsync("ReceiveResourceUpdate");

            return NoContent();
        }

        // USUWANIE (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null) return NotFound();

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            // Powiadom wszystkich o usunięciu
            await _hubContext.Clients.All.SendAsync("ReceiveResourceUpdate");

            return NoContent();
        }
    }
}
