using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JKC.Backend.Infraestructura.Data.Repositorios;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using JKC.Backend.Infraestructura.Data.EntityFramework;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Aplicacion.Services.ProductosServices;
using JKC.Backend.Dominio.Entidades.Productos;
//using Register.Models;

var builder = WebApplication.CreateBuilder(args);

// Registro Servicios
builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();
builder.Services.AddScoped<IServicioProductos, ServicioProductos>();

//Registro Respositorios
builder.Services.AddScoped<IRepository<Usuarios>, Repository<Usuarios>>();
builder.Services.AddScoped<IRepository<Productos>, Repository<Productos>>();
builder.Services.AddControllers();

// Configura DbContext con la cadena de conexión de la configuración
builder.Services.AddDbContext<CommonDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configura CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins", policy =>
  {
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
  });
});

var app = builder.Build();

// Configura el middleware HTTP.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Usa la política de CORS
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
