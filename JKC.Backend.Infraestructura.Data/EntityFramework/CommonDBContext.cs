using JKC.Backend.Dominio.Entidades;
using JKC.Backend.Dominio.Entidades.Categorias;
using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Bodegas;
using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Usuario;
using Microsoft.EntityFrameworkCore;
using JKC.Backend.Dominio.Entidades.Movimientos;

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
    public DbSet<Bodega> Bodegas { get; set; }

    // Add this missing DbSet for the Categoria entity
    public DbSet<Categoria> Categorias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Esquema Seguridad
      modelBuilder.Entity<Usuario>().ToTable("usuarios", "seguridad");
      modelBuilder.Entity<Roles>().ToTable("roles", "seguridad");
      //modelBuilder.Entity<RolPermiso>().ToTable("rolpermiso", "seguridad");
      modelBuilder.Entity<RolesUsuario>().ToTable("roles_usuarios", "seguridad");
      modelBuilder.Entity<AsignarPermisos>().ToTable("rolpermiso", "seguridad");


      modelBuilder.Entity<Bodega>().ToTable("bodegas", "bodegas");
      // Esquema Productos
      modelBuilder.Entity<Producto>().ToTable("productos", "productos");

      modelBuilder.Entity<Categoria>().ToTable("categorias", "categorias");


      modelBuilder.Entity<TiposDocumento>().ToTable("tiposdocumento", "generales");
      modelBuilder.Entity<TiposTercero>().ToTable("tipostercero", "generales");
      modelBuilder.Entity<TiposPersona>().ToTable("tipospersona", "generales");
      modelBuilder.Entity<Pais>().ToTable("pais", "generales");
      modelBuilder.Entity<Departamento>().ToTable("departamento", "generales");
      modelBuilder.Entity<Ciudad>().ToTable("ciudad", "generales");
      modelBuilder.Entity<Tercero>().ToTable("tercero", "generales");
      modelBuilder.Entity<TiposMovimiento>().ToTable("tiposmovimiento", "dbo");
      modelBuilder.Entity<Movimiento>().ToTable("Movimientos", "dbo");

      modelBuilder.Entity<Modulo>()
      .ToTable("modulos", "seguridad") // Tabla modulos en esquema seguridad
      .HasKey(m => m.Id); // Asegúrate de definir la clave primaria

      modelBuilder.Entity<SubModulo>()
          .ToTable("submodulo", "seguridad") // Tabla submodulo en esquema seguridad
          .HasKey(s => s.IdSubModulo); // Asegúrate de definir la clave primaria

      modelBuilder.Entity<Modulo>()
          .HasMany(m => m.Submodulos)           // Un módulo tiene muchos submódulos
          .WithOne(s => s.Modulo)               // Cada submódulo tiene un módulo
          .HasForeignKey(s => s.IdModulo)       // Clave foránea en Submodulo
          .OnDelete(DeleteBehavior.Restrict);    // Opcional: evita borrado en cascada

      base.OnModelCreating(modelBuilder);
    }
  }
}
