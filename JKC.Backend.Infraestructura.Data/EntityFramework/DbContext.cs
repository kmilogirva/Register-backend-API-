using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JKC.Backend.Infraestructura.Data.EntityFramework
{
  public class CommonDBContext : DbContext
  {

    public CommonDBContext(DbContextOptions<CommonDBContext> options) : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //  if (!optionsBuilder.IsConfigured)
    //  {
    //    var connectionString = _configuration.GetConnectionString("MyDBConnection");
    //    optionsBuilder.UseSqlServer(connectionString);
    //  }
    //}

    // Agrega tus DbSets aquí, por ejemplo:
    public DbSet<Usuarios> Usuarios { get; set; }
    public DbSet<RolesUsuario> RolesUsuarios { get; set; }



    public DbSet<Productos> Productos { get; set; }
    //public DbSet<Login> Login { get; set; }

    // Otras DbSets...
    // public DbSet<OtroModelo> Otros { get; set; }
  }
}
