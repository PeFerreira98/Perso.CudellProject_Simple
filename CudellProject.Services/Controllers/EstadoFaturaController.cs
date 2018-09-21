using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CudellProject.Data.Contexts;
using CudellProject.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CudellProject.Services.Controllers
{
    [Produces("application/json")]
    [Route("api/EstadoFatura")]
    public class EstadoFaturaController : Controller
    {
        private readonly DemoDbContext _context;
        readonly ILogger<EstadoFaturaController> _logger;

        public EstadoFaturaController(DemoDbContext context, ILogger<EstadoFaturaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EstadoFatura
        [HttpGet]
        public IEnumerable<EstadoFatura> GetEstadoFatura()
        {
            _logger.LogInformation("EstadoFaturaController - GetEstadoFatura - Method Call");

            return _context.EstadoFatura;
        }

        // GET: api/EstadoFatura/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstadoFatura([FromRoute] short id)
        {
            _logger.LogInformation("EstadoFaturaController - GetEstadoFatura (with Id = " + id + ") - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var estadoFatura = await _context.EstadoFatura.SingleOrDefaultAsync(m => m.EstadoFaturaID == id);

            if (estadoFatura == null)
            {
                return NotFound();
            }

            return Ok(estadoFatura);
        }

        // PUT: api/EstadoFatura/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoFatura([FromRoute] short id, [FromBody] EstadoFatura estadoFatura)
        {
            _logger.LogInformation("EstadoFaturaController - PutEstadoFatura (with Id = " + id + ") - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != estadoFatura.EstadoFaturaID)
            {
                return BadRequest();
            }

            _context.Entry(estadoFatura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoFaturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EstadoFatura
        [HttpPost]
        public async Task<IActionResult> PostEstadoFatura([FromBody] EstadoFatura estadoFatura)
        {
            _logger.LogInformation("EstadoFaturaController - PostEstadoFatura - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EstadoFatura.Add(estadoFatura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoFatura", new { id = estadoFatura.EstadoFaturaID }, estadoFatura);
        }

        // DELETE: api/EstadoFatura/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoFatura([FromRoute] short id)
        {
            _logger.LogInformation("EstadoFaturaController - DeleteEstadoFatura (with Id = " + id + ") - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var estadoFatura = await _context.EstadoFatura.SingleOrDefaultAsync(m => m.EstadoFaturaID == id);
            if (estadoFatura == null)
            {
                return NotFound();
            }

            _context.EstadoFatura.Remove(estadoFatura);
            await _context.SaveChangesAsync();

            return Ok(estadoFatura);
        }

        private bool EstadoFaturaExists(short id)
        {
            _logger.LogInformation("EstadoFaturaController - EstadoFaturaExists (with Id = " + id + ") - Method Call");

            return _context.EstadoFatura.Any(e => e.EstadoFaturaID == id);
        }
    }
}