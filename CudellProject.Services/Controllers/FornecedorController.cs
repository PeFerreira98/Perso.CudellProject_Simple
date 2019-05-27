using CudellProject.Data.Contexts;
using CudellProject.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CudellProject.Services.Controllers
{
    [Produces("application/json")]
    [Microsoft.AspNetCore.Mvc.Route("api/Fornecedor")]
    public class FornecedorController : Controller
    {
        private readonly DemoDbContext _context;
        readonly ILogger<FornecedorController> _logger;

        public FornecedorController(DemoDbContext context, ILogger<FornecedorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Fornecedor
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IEnumerable<Fornecedor> GetFornecedor()
        {
            _logger.LogInformation("FornecedorController - GetFornecedor - Method Call");

            return _context.Fornecedor;
        }

        // GET: api/Fornecedor/5
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public async Task<IActionResult> GetFornecedor([FromRoute] short id)
        {
            _logger.LogInformation("FornecedorController - GetFornecedor (with id = " + id + " - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fornecedor = await _context.Fornecedor.SingleOrDefaultAsync(m => m.FornecedorID == id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return Ok(fornecedor);
        }

        // GET: api/Fornecedor/name
        [Microsoft.AspNetCore.Mvc.HttpGet("{name}")]
        public async Task<IActionResult> GetFornecedor([FromRoute] string name)
        {
            _logger.LogInformation("FornecedorController - GetFornecedor (with name = " + name + " - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fornecedor = await _context.Fornecedor.SingleOrDefaultAsync(m => m.DescritivoFornecedor == name);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return Ok(fornecedor);
        }

        // PUT: api/Fornecedor/5
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedor([FromRoute] short id, [FromBody] Fornecedor fornecedor)
        {
            _logger.LogInformation("FornecedorController - PutFornecedor - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fornecedor.FornecedorID)
            {
                return BadRequest();
            }

            _context.Entry(fornecedor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorExists(id))
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

        // POST: api/Fornecedor
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> PostFornecedor([FromBody] Fornecedor fornecedor)
        {
            _logger.LogInformation("FornecedorController - PostFornecedor - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Fornecedor.Add(fornecedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFornecedor", new { id = fornecedor.FornecedorID }, fornecedor);
        }

        // DELETE: api/Fornecedor/5
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedor([FromRoute] short id)
        {
            _logger.LogInformation("FornecedorController - DeleteFornecedor - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fornecedor = await _context.Fornecedor.SingleOrDefaultAsync(m => m.FornecedorID == id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            _context.Fornecedor.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return Ok(fornecedor);
        }

        private bool FornecedorExists(short id)
        {
            _logger.LogInformation("FornecedorController - FornecedorExists - Method Call");

            return _context.Fornecedor.Any(e => e.FornecedorID == id);
        }
    }
}