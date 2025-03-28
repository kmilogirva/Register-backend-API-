using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace JKC.Backend.Infraestructura.Data.EntityFramework
{
  public class CommonDBContext : DbContext
  {
    public CommonDBContext(DbContextOptions<CommonDBContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Usuarios> Usuarios { get; set; }
    public DbSet<RolesUsuario> RolesUsuarios { get; set; }
    public DbSet<Productos> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Esquema Seguridad
      modelBuilder.Entity<Usuarios>().ToTable("usuarios", "seguridad");
      modelBuilder.Entity<RolesUsuario>().ToTable("roles_usuarios", "seguridad");

      // Esquema Productos
      modelBuilder.Entity<Productos>().ToTable("productos", "productos");

      base.OnModelCreating(modelBuilder);
    }
  }
}
