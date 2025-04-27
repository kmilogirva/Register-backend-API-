using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JKC.Backend.Infraestructura.Data.Repositorios;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using JKC.Backend.Infraestructura.Data.EntityFramework;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Aplicacion.Services.SeguridadService;
using JKC.Backend.Aplicacion.Services.ProductoServices;
using JKC.Backend.Aplicacion.Services.CategoriasServices;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

// Configurar DbContext con la conexión a SQL Server
builder.Services.AddDbContext<CommonDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

// Servicios y repositorios
builder.Services.AddScoped<IServicioSeguridad, ServicioSeguridad>();
builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();
builder.Services.AddScoped<IServicioProducto, ServicioProducto>();
builder.Services.AddScoped<IServicioCategoria, ServicioCategoria>();


//Registro de Repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins", policy =>
  {
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
  });
});

// Autenticación JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
      };
    });

builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "API JkcInventory",
    Version = "v1",
    Description = "Documentación de la API con Swagger"
  });

  // JWT en Swagger
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Ingrese 'Bearer [espacio] token' en el campo.",
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API JkcInventory v1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
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

app.UseAuthentication(); // Muy importante que vaya antes
app.UseAuthorization();

app.MapControllers();

app.Run();

//using JKC.Backend.Infraestructura.Data.EntityFramework;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// Obtener la clave secreta desde appsettings.json
//var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

//// Configurar DbContext y demás servicios
//builder.Services.AddDbContext<CommonDBContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//           .EnableSensitiveDataLogging()
//           .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

//// Configurar la autenticación JWT
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//      options.TokenValidationParameters = new TokenValidationParameters
//      {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        //ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        //ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(key) // Aquí se utiliza la clave secreta desde appsettings
//      };
//    });

//builder.Services.AddAuthorization();

//// Configurar CORS y demás servicios
//builder.Services.AddCors(options =>
//{
//  options.AddPolicy("AllowAllOrigins", policy =>
//  {
//    policy.AllowAnyOrigin()
//          .AllowAnyMethod()
//          .AllowAnyHeader();
//  });
//});

//builder.Services.AddControllers();

//// Swagger, etc...

//var app = builder.Build();

//// Middleware y configuración de la aplicación
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseCors("AllowAllOrigins");

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

