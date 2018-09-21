using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CudellProject.Data.Contexts;
using CudellProject.Data.DTOs;
using CudellProject.Data.Models;
using CudellProject.Data.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CudellProject.Services.Controllers
{
    [Produces("application/json")]
    [Route("api/Fatura")]
    public class FaturaController : Controller
    {
        private readonly DemoDbContext _context;
        readonly ILogger<FaturaController> _logger;

        public FaturaController(DemoDbContext context, ILogger<FaturaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("addFatura")]
        public async Task<IActionResult> AddFatura(AddFaturaViewModel faturaModel)
        {
            if (string.IsNullOrEmpty(faturaModel.DataFatura)
                || string.IsNullOrEmpty(faturaModel.DataVencimento)
                || string.IsNullOrEmpty(faturaModel.Fornecedor)
                || faturaModel.Valor <= 0.00) return BadRequest();

            Fornecedor fornecedor = await GetFornecedorByName(faturaModel.Fornecedor);
            if (fornecedor == null) return NotFound();

            try
            {
                var dataFatura = DateTime.ParseExact(faturaModel.DataFatura, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                var dataVencimento = DateTime.ParseExact(faturaModel.DataVencimento, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                if(dataVencimento < dataFatura || dataVencimento < DateTime.Now || dataFatura < DateTime.Now) return RedirectToAction("Error", "Routing");

                var newFatura = new Fatura(
                    dataFatura,
                    dataVencimento,
                    Convert.ToDecimal(faturaModel.Valor),
                    fornecedor.FornecedorID,
                    User.Identity.Name.Split('\\')[1],
                    User.Identity.Name.Split('\\')[1]);

                return await PostFatura(newFatura);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost("editFatura")]
        public async Task<IActionResult> EditFatura(EditFaturaViewModel faturaModel)
        {
            if (string.IsNullOrEmpty(faturaModel.EstadoFatura)
                || string.IsNullOrEmpty(faturaModel.Fornecedor)
                || faturaModel.FaturaID <= 0
                || faturaModel.Valor <= 0.00) return BadRequest();

            Fornecedor fornecedor = await GetFornecedorByName(faturaModel.Fornecedor);
            EstadoFatura estadoFatura = await GetEstadoFaturaByName(faturaModel.EstadoFatura);
            Fatura oldFatura = await GetFaturaById(faturaModel.FaturaID);
            if (fornecedor == null || estadoFatura == null || oldFatura == null) return NotFound();

            try
            {
                oldFatura.Valor = Convert.ToDecimal(faturaModel.Valor);
                oldFatura.FornecedorID = fornecedor.FornecedorID;
                oldFatura.AlterUser = User.Identity.Name.Split('\\')[1];
                oldFatura.AlterDate = DateTime.Now;
                oldFatura.EstadoFaturaID = estadoFatura.EstadoFaturaID;

                return await PutFatura(faturaModel.FaturaID, oldFatura);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        // PUT: api/Fatura/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFatura([FromRoute] long id, [FromBody] Fatura fatura)
        {
            _logger.LogInformation("FaturaController - PutFatura - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fatura.FaturaID)
            {
                return BadRequest();
            }

            _context.Entry(fatura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await PostAudit(new Audit()
            {
                OperationTimeStamp = DateTime.Now,
                Operation = "Operation:Update; Entity:Fatura, EntityID: " + fatura.FaturaID,
                Username = User.Identity.Name.Split('\\')[1]
            });
        }

        // POST: api/Fatura
        [HttpPost]
        public async Task<IActionResult> PostFatura([FromBody] Fatura fatura)
        {
            _logger.LogInformation("FaturaController - PostFatura - Method Call");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Fatura.Add(fatura);
            await _context.SaveChangesAsync();

            CreatedAtAction("GetFatura", new { id = fatura.FaturaID }, fatura);

            return await new AuditController(_context).PostAudit(new Audit()
                {
                    OperationTimeStamp = DateTime.Now,
                    Operation = "Operation:Create; Entity:Fatura, EntityID: " + fatura.FaturaID,
                    Username = User.Identity.Name.Split('\\')[1]
                });
        }

        private bool FaturaExists(long id)
        {
            _logger.LogInformation("FaturaController - FaturaExists - Method Call");

            return _context.Fatura.Any(e => e.FaturaID == id);
        }

        public async Task<Fornecedor> GetFornecedorByName(string nome)
        {
            _logger.LogInformation("AddFaturaController - GetFornecedorByName -  (with nome = " + nome + " - Method Call");
            return await _context.Fornecedor.SingleOrDefaultAsync(m => m.DescritivoFornecedor == nome);
        }

        private async Task<EstadoFatura> GetEstadoFaturaByName(string nome)
        {
            _logger.LogInformation("AddFaturaController - GetEstadoFaturaByName -  (with nome = " + nome + " - Method Call");
            return await _context.EstadoFatura.SingleOrDefaultAsync(m => m.DescritivoEstadoFatura == nome);
        }

        private async Task<Fatura> GetFaturaById(long id)
        {
            _logger.LogInformation("AddFaturaController - GetFaturaById -  (with id = " + id + " - Method Call");
            return await _context.Fatura
                .Include(m => m.Fornecedor)
                .Include(m => m.EstadoFatura)
                .SingleOrDefaultAsync(m => m.FaturaID == id);
        }

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