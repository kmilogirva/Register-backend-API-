using Jkc.Backend.Aplicacion.Services.EmailService;
using JKC.Backend.Aplicacion.Services.BodegaServices;
using JKC.Backend.Aplicacion.Services.CategoriasServices;
using JKC.Backend.Aplicacion.Services.GeneralesServices;
using JKC.Backend.Aplicacion.Services.ProductoServices;
using JKC.Backend.Aplicacion.Services.SeguridadService;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Services;
using JKC.Backend.Infraestructura.Data.EntityFramework;
using JKC.Backend.Infraestructura.Data.Repositorios;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

// 🔹 Configurar DbContext
builder.Services.AddDbContext<CommonDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

// 🔹 Servicios y repositorios
builder.Services.AddScoped<IServicioSeguridad, ServicioSeguridad>();
builder.Services.AddScoped<IServicioUsuario, ServicioUsuario>();
builder.Services.AddScoped<IServicioProducto, ServicioProducto>();
builder.Services.AddScoped<IServicioCategoria, ServicioCategoria>();
builder.Services.AddScoped<IServicioGeneral, ServicioGeneral>();
builder.Services.AddScoped<IServicioBodega, ServicioBodega>();
builder.Services.AddScoped<IServicioModulo, ServicioModulo>();
builder.Services.AddScoped<IServicioSubModulo, ServicioSubModulo>();
builder.Services.AddScoped<IServicioTercero, ServicioTercero>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();

// 🔹 CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins", policy =>
  {
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
  });
});

// 🔹 Autenticación JWT
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

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "API JkcInventory",
    Version = "v1",
    Description = "Documentación de la API con Swagger"
  });

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

// 🔹 IMPORTANTE: Configurar puerto dinámico para Azure
var port = Environment.GetEnvironmentVariable("PORT")
           ?? Environment.GetEnvironmentVariable("WEBSITES_PORT")
           ?? "8080"; // fallback
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// 🔹 Middleware
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API JkcInventory v1");
    c.RoutePrefix = string.Empty;
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 🔹 Endpoint raíz /health o /
app.MapGet("/", () => Results.Ok("API Running 🚀"));

app.Run();

Log.CloseAndFlush();
