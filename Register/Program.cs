using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using JKC.Backend.Infraestructura.Data.Repositorios;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using JKC.Backend.Infraestructura.Data.EntityFramework;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Aplicacion.Services.ProductosServices;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext con la conexión a SQL Server
builder.Services.AddDbContext<CommonDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging() // Muestra los valores en los logs
           .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

// Registro de servicios y repositorios
builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();
builder.Services.AddScoped<IServicioProductos, ServicioProductos>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins", policy =>
  {
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
  });
});

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "API JkcInventory",
    Version = "v1",
    Description = "Documentación de la API con Swagger"
  });

  //Dejarcomenatdo por ahora, útil para JWT
  // (Opcional) Agregar autenticación JWT a Swagger
  //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  //{
  //  Name = "Authorization",
  //  Type = SecuritySchemeType.Http,
  //  Scheme = "Bearer",
  //  BearerFormat = "JWT",
  //  In = ParameterLocation.Header,
  //  Description = "Ingrese 'Bearer [token]' en el campo para autenticar."
  //});

  //c.AddSecurityRequirement(new OpenApiSecurityRequirement
  //  {
  //      {
  //          new OpenApiSecurityScheme
  //          {
  //              Reference = new OpenApiReference
  //              {
  //                  Type = ReferenceType.SecurityScheme,
  //                  Id = "Bearer"
  //              }
  //          },
  //          new string[] {}
  //      }
  //  });
});

var app = builder.Build();

// Middleware HTTP y configuraciones
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API JkcInventory v1");
    c.RoutePrefix = string.Empty; // Swagger estará en la raíz
  });
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();
app.Run();
