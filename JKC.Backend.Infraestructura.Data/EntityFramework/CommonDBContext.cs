using JKC.Backend.Dominio.Entidades;
using JKC.Backend.Dominio.Entidades.Categorias;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Usuario;
using Microsoft.EntityFrameworkCore;

namespace JKC.Backend.Infraestructura.Data.EntityFramework
{
  public class CommonDBContext : DbContext
  {
    public CommonDBContext(DbContextOptions<CommonDBContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<RolesUsuario> RolesUsuarios { get; set; }
    public DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Esquema Seguridad
      modelBuilder.Entity<Usuario>().ToTable("usuarios1", "seguridad");
      modelBuilder.Entity<Roles>().ToTable("roles", "seguridad");
      modelBuilder.Entity<RolesUsuario>().ToTable("roles_usuarios", "seguridad");

      // Esquema Productos
      modelBuilder.Entity<Producto>().ToTable("productos", "productos");

      modelBuilder.Entity<Categoria>().ToTable("categorias", "categorias");
     
      base.OnModelCreating(modelBuilder);
    }
  }
}
