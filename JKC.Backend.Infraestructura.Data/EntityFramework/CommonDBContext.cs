using JKC.Backend.Dominio.Entidades;
using JKC.Backend.Dominio.Entidades.Categorias;
using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Bodegas;
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
    public DbSet<Bodega> Bodegas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Esquema Seguridad
      modelBuilder.Entity<Usuario>().ToTable("usuarios1", "seguridad");
      modelBuilder.Entity<Roles>().ToTable("roles", "seguridad");
      //modelBuilder.Entity<RolPermiso>().ToTable("rolpermiso", "seguridad");
      modelBuilder.Entity<RolesUsuario>().ToTable("roles_usuarios", "seguridad");
      modelBuilder.Entity<AsignarPermisos>().ToTable("rolpermiso", "seguridad");
      

      modelBuilder.Entity<Bodega>().ToTable("bodegas", "bodegas");
      // Esquema Productos
      modelBuilder.Entity<Producto>().ToTable("productos", "productos");

      modelBuilder.Entity<Categoria>().ToTable("categorias", "categorias");
 


      modelBuilder.Entity<TiposDocumento>().ToTable("tiposdocumento", "generales");

      modelBuilder.Entity<Modulo>()
     .ToTable("modulos", "seguridad") // Tabla modulos en esquema seguridad
     .HasKey(m => m.Id); // Aseg�rate de definir la clave primaria

      modelBuilder.Entity<SubModulo>()
          .ToTable("submodulo", "seguridad") // Tabla submodulo en esquema seguridad
          .HasKey(s => s.IdSubModulo); // Aseg�rate de definir la clave primaria

      modelBuilder.Entity<Modulo>()
          .HasMany(m => m.Submodulos)           // Un m�dulo tiene muchos subm�dulos
          .WithOne(s => s.Modulo)               // Cada subm�dulo tiene un m�dulo
          .HasForeignKey(s => s.IdModulo)       // Clave for�nea en Submodulo
          .OnDelete(DeleteBehavior.Restrict);   // Opcional: evita borrado en cascada


      base.OnModelCreating(modelBuilder);
    }
  }
}
