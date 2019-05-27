using CudellProject.Data.Contexts;
using CudellProject.Data.Models;
using CudellProject.Data.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CudellProject.Services.Controllers
{
    public class RoutingController : Controller
    {
        private readonly DemoDbContext _context;
        readonly ILogger<RoutingController> _logger;

        public RoutingController(DemoDbContext context, ILogger<RoutingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult Table()
        {
            return View();
        }

        public async Task<IActionResult> AddFatura()
        {
            System.Collections.Generic.List<Fornecedor> fornecedores = await _context.Fornecedor.ToListAsync();
            AddFaturaViewModel model = new AddFaturaViewModel();

            foreach (Fornecedor f in fornecedores)
            {
                model.Fornecedores.Add(new SelectListItem { Text = f.DescritivoFornecedor });
            }

            return View(model);
        }

        public async Task<IActionResult> EditFatura([FromRoute] long id)
        {
            var fatura = await _context.Fatura.Include(m => m.Fornecedor).Include(m => m.EstadoFatura).SingleOrDefaultAsync(m => m.FaturaID == id);
            System.Collections.Generic.List<EstadoFatura> estadosFatura = await _context.EstadoFatura.ToListAsync();
            System.Collections.Generic.List<Fornecedor> fornecedores = await _context.Fornecedor.ToListAsync();

            EditFaturaViewModel model = new EditFaturaViewModel
            {
                FaturaID = id,
                Valor = Convert.ToDouble(fatura.Valor),
                Fornecedor = fatura.Fornecedor.DescritivoFornecedor,
                EstadoFatura = fatura.EstadoFatura.DescritivoEstadoFatura
            };

            foreach (EstadoFatura e in estadosFatura)
            {
                model.EstadosFatura.Add(new SelectListItem { Text = e.DescritivoEstadoFatura });
            }

            foreach (Fornecedor f in fornecedores)
            {
                model.Fornecedores.Add(new SelectListItem { Text = f.DescritivoFornecedor });
            }

            return View(model);
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult NotImplemented()
        {
            return View();
        }

        public IActionResult OperationSuccessful()
        {
            return View();
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
