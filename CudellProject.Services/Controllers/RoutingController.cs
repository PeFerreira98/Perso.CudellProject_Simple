using CudellProject.Data.Contexts;
using CudellProject.Data.Models;
using CudellProject.Data.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [AllowAnonymous]
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
            var fornecedores = await _context.Fornecedor.ToListAsync();
            var model = new AddFaturaViewModel();
            foreach (Fornecedor f in fornecedores) model.Fornecedores.Add(new SelectListItem { Text = f.DescritivoFornecedor });

            return View(model);
        }

        public async Task<IActionResult> EditFatura([FromRoute] long id)
        {
            var fatura = await _context.Fatura.Include(m => m.Fornecedor).Include(m => m.EstadoFatura).SingleOrDefaultAsync(m => m.FaturaID == id);
            var estadosFatura = await _context.EstadoFatura.ToListAsync();
            var fornecedores = await _context.Fornecedor.ToListAsync();

            var model = new EditFaturaViewModel
            {
                FaturaID = id,
                Valor = Convert.ToDouble(fatura.Valor),
                Fornecedor = fatura.Fornecedor.DescritivoFornecedor,
                EstadoFatura = fatura.EstadoFatura.DescritivoEstadoFatura
            };

            foreach (EstadoFatura e in estadosFatura) model.EstadosFatura.Add(new SelectListItem { Text = e.DescritivoEstadoFatura });
            foreach (Fornecedor f in fornecedores) model.Fornecedores.Add(new SelectListItem { Text = f.DescritivoFornecedor });

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult NotImplemented()
        {
            return View();
        }

        public IActionResult OperationSuccessful()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
