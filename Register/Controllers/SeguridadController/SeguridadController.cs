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
using Microsoft.AspNetCore.Identity;
using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Generales;

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


    [Authorize]
    [HttpPost("crearrol")]
    public async Task<IActionResult> CrearRol([FromBody] Rol rol)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        Rol nuevorol = await _seguridadServicio.CrearRol(rol);


        if (nuevorol is null)
          return Conflict(new { mensaje = "El rol ya existe." });

        //if (nuevorol is null)
        //{ return BadRequest(new { mensaje = "No se pudo crear el rol." });
        //}

        return Ok(new
        {
          mensaje = "Rol creado exitosamente.",
          rol = nuevorol
        });

      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          mensaje = "Ocurrió un error al crear el rol.",
          detalle = ex.Message
        });
      }
    }


    [HttpGet("combo-roles")]
    public async Task<IActionResult> ObtenerComboRoles()
    {
      try
      {
        var roles = await _seguridadServicio.ObtenerListadoRoles();

        // Mapear a ComboResponse
        var combo = roles.Select(r => new ComboResponse
        {
          Codigo = r.Id,
          Valor = r.NombreRol
        });

        return Ok(combo);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al obtener los roles.", error = ex.Message });
      }
    }

    [HttpGet("consultar-permisos-accion-por-rol/{idRol}")]
    public async Task<IActionResult> ObtenerPermisosPorRol(int idRol)
    {
      var resultado = await _seguridadServicio.ObtenerPermisosPorRol(idRol);
      return Ok(resultado);
    }




    [HttpPost("asignar-permisos-a-rol")]
    public async Task<IActionResult> AsignarPermisosARol([FromBody] List<AsignarPermisos> request)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        var resultado = await _seguridadServicio.CrearPermisosRolesAcciones(request);
        return Ok(new { mensaje = "Permisos asignados exitosamente.", resultado });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al asignar permisos al rol.", error = ex.Message });
      }
    }
  }
}
