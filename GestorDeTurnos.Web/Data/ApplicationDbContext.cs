using GestorDeTurnos.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorDeTurnos.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Afiliados> Afiliados { get; set; }
        //public DbSet<Caja> Cajas { get; set; }
        //public DbSet<Funcionario> Funcionarios { get; set; }
        //public DbSet<TV> TVs { get; set; }
        //public DbSet<Turnos> Turnos { get; set; }
    }
}