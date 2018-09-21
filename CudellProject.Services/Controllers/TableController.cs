using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CudellProject.Data.Contexts;
using CudellProject.Data.DTOs;
using CudellProject.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CortesProject.Services.Controllers
{
    public class TableController : Controller
    {
        private readonly DemoDbContext _context;
        readonly ILogger<TableController> _logger;

        private int firstTableLength = 0;
        private int secondTableLength = 0;

        public TableController(DemoDbContext context, ILogger<TableController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> LoadFirstTableData()
        {
            _logger.LogInformation("TableController - LoadFirstTableData - Method Call");

            try
            {
                var currentUser = User.Identity.Name.Split('\\')[1];
                var data = new List<FaturaPendenteDTO>();
                var faturas = new List<Fatura>();
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var orderDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var orderColumn = Request.Form["columns[" + Request.Form["order[0][Column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var startItem = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                int start = startItem != null ? Convert.ToInt32(startItem) : 0;
                int end = length != null ? Convert.ToInt32(length) : 0;

                _logger.LogInformation("TableController - LoadFirstTableData - Variables Successfully Instantiated");
                
                if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrEmpty(orderColumn))
                {
                    _logger.LogInformation("TableController - LoadFirstTableData - Entering Sorted Search (with value = " + searchValue + " and " + orderColumn + " and " + orderDirection + ")");
                    faturas = GetCurrentUserFaturas(currentUser, "Por Aprovar", start, end, searchValue, orderColumn, orderDirection).Result;
                    data = ParsingDataResultFPDTO(faturas);
                    return Json(new { draw = draw, recordsFiltered = firstTableLength, recordsTotal = firstTableLength, data = data });
                }
                
                if (!string.IsNullOrEmpty(searchValue))
                {
                    _logger.LogInformation("TableController - LoadFirstTableData - Entering Search (with value = " + searchValue + ")");
                    faturas = GetCurrentUserFaturas(currentUser, "Por Aprovar", start, end, searchValue).Result;
                    data = ParsingDataResultFPDTO(faturas);
                    return Json(new { draw = draw, recordsFiltered = firstTableLength, recordsTotal = firstTableLength, data = data });
                }

                if (!string.IsNullOrEmpty(orderColumn))
                {
                    _logger.LogInformation("TableController - LoadFirstTableData - Entering Sort (with value = " + orderColumn + " and " + orderDirection + ")");
                    faturas = GetCurrentUserFaturas(currentUser, "Por Aprovar", start, end, orderColumn, orderDirection).Result;
                    data = ParsingDataResultFPDTO(faturas);
                    return Json(new { draw = draw, recordsFiltered = firstTableLength, recordsTotal = firstTableLength, data = data });
                }

                _logger.LogInformation("TableController - LoadFirstTableData - Returning all Results");
                faturas = GetCurrentUserFaturas(currentUser, "Por Aprovar", start, end).Result;
                data = ParsingDataResultFPDTO(faturas);
                return Json(new { draw = draw, recordsFiltered = firstTableLength, recordsTotal = firstTableLength, data = data });
            }
            catch (Exception e)
            {
                _logger.LogInformation("TableController - LoadFirstTableData - Exception detected with error = " + e + ")");
                throw;
            }
        }

        public async Task<IActionResult> LoadSecondTableData()
        {
            _logger.LogInformation("TableController - LoadSecondTableData - Method Call");

            try
            {
                var currentUser = User.Identity.Name.Split('\\')[1];
                var data = new List<OwnFaturaDTO>();
                var faturas = new List<Fatura>();
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var orderDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var orderColumn = Request.Form["columns[" + Request.Form["order[0][Column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var startItem = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                int start = startItem != null ? Convert.ToInt32(startItem) : 0;
                int end = length != null ? Convert.ToInt32(length) : 0;

                _logger.LogInformation("TableController - LoadSecondTableData - Variables Successfully Instantiated");

                if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrEmpty(orderColumn))
                {
                    _logger.LogInformation("TableController - LoadSecondTableData - Entering Sorted Search (with value = " + searchValue + " and " + orderColumn + " and " + orderDirection + ")");
                    faturas = GetCurrentUserFaturas(currentUser, start, end, searchValue, orderColumn, orderDirection).Result;
                    data = ParsingDataResultOFDTO(faturas);
                    return Json(new { draw = draw, recordsFiltered = secondTableLength, data = data });
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    _logger.LogInformation("TableController - LoadSecondTableData - Entering Search (with value = " + searchValue + ")");
                    faturas = GetCurrentUserFaturas(currentUser, start, end, searchValue).Result;
                    data = ParsingDataResultOFDTO(faturas);
                    return Json(new { draw = draw, recordsFiltered = secondTableLength, data = data });
                }

                if (!string.IsNullOrEmpty(orderColumn))
                {
                    _logger.LogInformation("TableController - LoadSecondTableData - Entering Sort (with value = " + orderColumn + " and " + orderDirection + ")");
                    faturas = GetCurrentUserFaturas(currentUser, start, end, orderColumn, orderDirection).Result;
                    data = ParsingDataResultOFDTO(faturas);
                    return Json(new { draw = draw, recordsFiltered = secondTableLength, data = data });
                }
                
                _logger.LogInformation("TableController - LoadSecondTableData - Returning all Results");
                faturas = GetCurrentUserFaturas(currentUser, start, end).Result;
                data = ParsingDataResultOFDTO(faturas);
                return Json(new { draw = draw, recordsFiltered = secondTableLength, data = data });
            }
            catch (Exception e)
            {
                _logger.LogInformation("TableController - LoadSecondTableData - Exception detected with error = " + e + ")");
                throw;
            }
        }


        /* Game On */
        private async Task<List<Fatura>> GetCurrentUserFaturas(string currentUser, int start, int end)
        {
            secondTableLength = await _context.Fatura.Where(fatura => fatura.AlterUser == currentUser).CountAsync();
            return await _context.Fatura
                        .Include(fatura => fatura.Fornecedor)
                        .Include(fatura => fatura.EstadoFatura)
                        .Where(fatura => fatura.AlterUser == currentUser)
                        .Skip(start)
                        .Take(end)
                        .ToListAsync();
        }
        private async Task<List<Fatura>> GetCurrentUserFaturas(string currentUser, int start, int end, string searchValue)
        {
            secondTableLength = await _context.Fatura.Where(fatura => fatura.AlterUser == currentUser && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue) || fatura.Valor.ToString().Contains(searchValue) || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue))).CountAsync();
            return await _context.Fatura
                        .Include(i => i.Fornecedor)
                        .Include(fatura => fatura.EstadoFatura)
                        .Where(fatura => fatura.AlterUser == currentUser
                            && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                            //|| fatura.DataFatura.ToString().Contains(searchValue)
                            //|| fatura.DataVencimento.ToString().Contains(searchValue)
                            || fatura.Valor.ToString().Contains(searchValue)
                            || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                        .Skip(start)
                        .Take(end)
                        .ToListAsync();
        }
        private async Task<List<Fatura>> GetCurrentUserFaturas(string currentUser, int start, int end, string orderColumn, string orderDirection)
        {
            secondTableLength = await _context.Fatura.Where(fatura => fatura.AlterUser == currentUser).CountAsync();
            if (orderColumn == "fornecedor")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderBy(fatura => fatura.Fornecedor.DescritivoFornecedor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderByDescending(fatura => fatura.Fornecedor.DescritivoFornecedor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "dataFatura")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderBy(fatura => fatura.DataFatura)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderByDescending(fatura => fatura.DataFatura)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "dataVencimento")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderBy(fatura => fatura.DataVencimento)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderByDescending(fatura => fatura.DataVencimento)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "valor")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderBy(fatura => fatura.Valor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderByDescending(fatura => fatura.Valor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "estado")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderBy(fatura => fatura.EstadoFaturaID)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser)
                                                            .OrderByDescending(fatura => fatura.EstadoFaturaID)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }

            return await _context.Fatura
                        .Include(i => i.Fornecedor)
                        .Include(fatura => fatura.EstadoFatura)
                        .Where(fatura => fatura.AlterUser == currentUser)
                        .Skip(start)
                        .Take(end)
                        .ToListAsync();
        }
        private async Task<List<Fatura>> GetCurrentUserFaturas(string currentUser, int start, int end, string searchValue, string orderColumn, string orderDirection)
        {
            secondTableLength = await _context.Fatura.Where(fatura => fatura.AlterUser == currentUser && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue) || fatura.Valor.ToString().Contains(searchValue) || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue))).CountAsync();
            if (orderColumn == "fornecedor")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.Fornecedor.DescritivoFornecedor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.Fornecedor.DescritivoFornecedor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "dataFatura")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.DataFatura)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.DataFatura)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "dataVencimento")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.DataVencimento)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.DataVencimento)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "valor")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.Valor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.Valor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "estado")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.EstadoFaturaID)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();

                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Include(fatura => fatura.EstadoFatura)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)
                                                                || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.EstadoFaturaID)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }

            return await _context.Fatura
                        .Include(i => i.Fornecedor)
                        .Include(fatura => fatura.EstadoFatura)
                        .Where(fatura => fatura.AlterUser == currentUser
                            && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                            //|| fatura.DataFatura.ToString().Contains(searchValue)
                            //|| fatura.DataVencimento.ToString().Contains(searchValue)
                            || fatura.Valor.ToString().Contains(searchValue)
                            || fatura.EstadoFatura.DescritivoEstadoFatura.Contains(searchValue)))
                        .Skip(start)
                        .Take(end)
                        .ToListAsync();
        }


        private async Task<List<Fatura>> GetCurrentUserFaturas(string currentUser, string estadoFatura, int start, int end)
        {
            firstTableLength = await _context.Fatura.Where(fatura => fatura.AlterUser == currentUser && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura).CountAsync();
            return await _context.Fatura
                        .Include(fatura => fatura.Fornecedor)
                        .Where(fatura => fatura.AlterUser == currentUser
                            && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                        .Skip(start)
                        .Take(end)
                        .ToListAsync();
        }
        private async Task<List<Fatura>> GetCurrentUserFaturas(string currentUser, string estadoFatura, int start, int end, string searchValue)
        {
            firstTableLength = await _context.Fatura.Where(fatura => fatura.AlterUser == currentUser && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue) || fatura.Valor.ToString().Contains(searchValue))).CountAsync();
            return await _context.Fatura
                        .Include(i => i.Fornecedor)
                        .Where(fatura => fatura.AlterUser == currentUser
                            && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                            && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                            //|| fatura.DataFatura.ToString().Contains(searchValue)
                            //|| fatura.DataVencimento.ToString().Contains(searchValue)
                            || fatura.Valor.ToString().Contains(searchValue)))
                        .Skip(start)
                        .Take(end)
                        .ToListAsync();
        }
        private async Task<List<Fatura>> GetCurrentUserFaturas(string currentUser, string estadoFatura, int start, int end, string orderColumn, string orderDirection)
        {
            firstTableLength = await _context.Fatura.Where(fatura => fatura.AlterUser == currentUser && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura).CountAsync();
            if (orderColumn == "fornecedor")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderBy(fatura => fatura.Fornecedor.DescritivoFornecedor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderByDescending(fatura => fatura.Fornecedor.DescritivoFornecedor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "dataFatura")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderBy(fatura => fatura.DataFatura)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderByDescending(fatura => fatura.DataFatura)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "dataVencimento")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderBy(fatura => fatura.DataVencimento)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderByDescending(fatura => fatura.DataVencimento)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "valor")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderBy(fatura => fatura.Valor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderByDescending(fatura => fatura.Valor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "estado")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderBy(fatura => fatura.EstadoFaturaID)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                                                            .OrderByDescending(fatura => fatura.EstadoFaturaID)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }

            return await _context.Fatura
                        .Include(i => i.Fornecedor)
                        .Where(fatura => fatura.AlterUser == currentUser
                            && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura)
                        .Skip(start)
                        .Take(end)
                        .ToListAsync();
        }
        private async Task<List<Fatura>> GetCurrentUserFaturas(string currentUser, string estadoFatura, int start, int end, string searchValue, string orderColumn, string orderDirection)
        {
            firstTableLength = await _context.Fatura.Where(fatura => fatura.AlterUser == currentUser && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue) || fatura.Valor.ToString().Contains(searchValue))).CountAsync();
            if (orderColumn == "fornecedor")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString("dd-MM-yyyy").Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString("dd-MM-yyyy").Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.Fornecedor.DescritivoFornecedor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                || fatura.DataFatura.ToString("dd-MM-yyyy").Contains(searchValue)
                                                                || fatura.DataVencimento.ToString("dd-MM-yyyy").Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.Fornecedor.DescritivoFornecedor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "dataFatura")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.DataFatura)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.DataFatura)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "dataVencimento")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.DataVencimento)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.DataVencimento)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "valor")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.Valor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.Valor)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            if (orderColumn == "estado")
            {
                if (orderDirection == "asc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderBy(fatura => fatura.EstadoFaturaID)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
                if (orderDirection == "desc") return await _context.Fatura
                                                            .Include(i => i.Fornecedor)
                                                            .Where(fatura => fatura.AlterUser == currentUser
                                                                && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                                                                && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                                                                //|| fatura.DataFatura.ToString().Contains(searchValue)
                                                                //|| fatura.DataVencimento.ToString().Contains(searchValue)
                                                                || fatura.Valor.ToString().Contains(searchValue)))
                                                            .OrderByDescending(fatura => fatura.EstadoFaturaID)
                                                            .Skip(start)
                                                            .Take(end)
                                                            .ToListAsync();
            }
            
            return await _context.Fatura
                        .Include(i => i.Fornecedor)
                        .Where(fatura => fatura.AlterUser == currentUser
                            && fatura.EstadoFatura.DescritivoEstadoFatura == estadoFatura
                            && (fatura.Fornecedor.DescritivoFornecedor.Contains(searchValue)
                            //|| fatura.DataFatura.ToString().Contains(searchValue)
                            //|| fatura.DataVencimento.ToString().Contains(searchValue)
                            || fatura.Valor.ToString().Contains(searchValue)))
                        .Skip(start)
                        .Take(end)
                        .ToListAsync();
        }


        private List<FaturaPendenteDTO> ParsingDataResultFPDTO(List<Fatura> faturas)
        {
            var data = new List<FaturaPendenteDTO>();

            foreach (Fatura fatura in faturas)
            {
                data.Add(new FaturaPendenteDTO(fatura.FaturaID, fatura.Fornecedor.DescritivoFornecedor, fatura.DataFatura, fatura.DataVencimento, (double)fatura.Valor));
            }

            return data;
        }
        private List<OwnFaturaDTO> ParsingDataResultOFDTO(List<Fatura> faturas)
        {
            var data = new List<OwnFaturaDTO>();

            foreach (Fatura fatura in faturas)
            {
                data.Add(new OwnFaturaDTO(fatura.Fornecedor.DescritivoFornecedor, fatura.DataFatura, fatura.DataVencimento, (double)fatura.Valor, fatura.EstadoFatura.DescritivoEstadoFatura));
            }

            return data;
        }
        
    }
}