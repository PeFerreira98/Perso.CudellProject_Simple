using CudellProject.Data.Contexts;
using CudellProject.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CudellProject.Services.Controllers
{
    [Produces("application/json")]
    [Route("api/Audit")]
    public class AuditController : Controller
    {
        private readonly DemoDbContext _context;

        public AuditController(DemoDbContext context)
        {
            _context = context;
        }

        // POST: api/Audit
        [HttpPost]
        public async Task<IActionResult> PostAudit([FromBody] Audit audit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Audit.Add(audit);
            await _context.SaveChangesAsync();

            CreatedAtAction("GetAudit", new { id = audit.AuditID }, audit);

            return RedirectToAction("OperationSuccessful", "Routing");
        }
    }
}