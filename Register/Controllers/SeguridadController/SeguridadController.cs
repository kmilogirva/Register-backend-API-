using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Aplicacion.Services.SeguridadService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace JKC.Backend.WebApi.Controllers.SeguridadController
{
  [ApiController]
  [Route("api/[controller]")]

  public class SeguridadController : ControllerBase
  {
    private readonly IServicioSeguridad _seguridadServicio;
    private readonly IConfiguration _configuration;

    public SeguridadController(IServicioSeguridad seguridadServicio, IConfiguration configuration)
    {
      _seguridadServicio = seguridadServicio;
      _configuration = configuration;
    }

    [HttpPost("InicioSesion")]
    public async Task<IActionResult> Login([FromBody] Login credenciales)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      // Llamamos al servicio de login
      var resultado = await _seguridadServicio.LoginAsync(credenciales.Correo, credenciales.Contrasena);

      // Si el resultado no fue exitoso, regresamos Unauthorized
      if (!resultado.Exitoso)
      {
        return Unauthorized(new
        {
          Exitoso = false,
          Mensaje = resultado.Mensaje ?? "Credenciales inválidas"
        });
      }

      // Si el resultado fue exitoso, generamos el JWT
      var claveSecreta = _configuration["Jwt:Key"];
      if (string.IsNullOrEmpty(claveSecreta))
      {
        throw new InvalidOperationException("La clave JWT no está configurada. Verifica el appsettings.json.");
      }
      var key = Encoding.UTF8.GetBytes(claveSecreta);

      var claims = new[]
      {
       new Claim("IdUsuario", resultado.Data.IdUsuario.ToString()), // Usamos resultado.Data para obtener el UsuarioDto
        new Claim(ClaimTypes.Name, resultado.Data.Nombre),
        new Claim(ClaimTypes.Email, resultado.Data.Correo)
        // Aquí puedes agregar más claims si es necesario, por ejemplo, para roles
      };

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(2),
        Issuer = _configuration["Jwt:Issuer"],
        Audience = _configuration["Jwt:Audience"],
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);

      // Retornamos el token y la información del usuario
      return Ok(new
      {
        Token = tokenString,
        Expira = tokenDescriptor.Expires,
        Usuario = new
        {
          resultado.Data.IdUsuario,
          resultado.Data.Nombre,
          resultado.Data.Correo
        }
      });
    }

    // El propósito de este endpoint es para consultar los permisos que tiene cada usuario en el sistema según el rol asignado y mostrarlos en el frontend.
    [Authorize]
    [HttpGet("consultarpermisosusuario/{idUsuario}")]
    public async Task<IActionResult> ObtenerPermisosUsuario(int idUsuario)
    {
      try
      {
        var permisos = await _seguridadServicio.ObtenerPermisosPorIdUsuario(idUsuario);

        if (permisos == null || !permisos.Any())
          return NotFound(new { mensaje = "No se encontraron permisos para este usuario." });

        var modulosAgrupados = permisos
          .GroupBy(p => p.IdModuloPadre)
          .Select(g => new
          {
            ModuloPadre = g.FirstOrDefault().NomModuloPadre,
            ModulosHijos = g.Select(m => new
            {
              m.IdModulo,
              m.NomModulo,
              m.IdPermiso,
              m.NomPermiso
            }).ToList()
          }).ToList();

        return Ok(modulosAgrupados);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          mensaje = "Ocurrió un error al consultar los permisos del usuario.",
          detalle = ex.Message
        });
      }
    }
  }
}
