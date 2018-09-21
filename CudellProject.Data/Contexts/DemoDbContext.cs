using CudellProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CudellProject.Data.Contexts
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
        }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<EstadoFatura> EstadoFatura { get; set; }
        public DbSet<Fatura> Fatura { get; set; }
        public DbSet<Audit> Audit { get; set; }
    }
}
