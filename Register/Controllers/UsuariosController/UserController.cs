using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using Microsoft.AspNetCore.Mvc;


namespace JKC.Backend.WebApi.Controllers.UsuariosController
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IServicioUsuario _usuarioServicio;

    public UserController(IServicioUsuario usuarioServicio)
    {
      _usuarioServicio = usuarioServicio;
    }

    // POST: api/User/Create
    [HttpPost("CreateUsers")]
    public async Task<IActionResult> CreateUsers([FromBody] Usuarios nuevoUsuario)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest("Datos inválidos");
      }

      nuevoUsuario.FechaCrea = DateTime.Now;

      var resultado = await _usuarioServicio.RegistrarUsuarioAsync(nuevoUsuario);

      if (resultado.Exitoso)
      {
        return Ok(new ResponseMessages
        {
          Exitoso = true,
          Mensaje = "Cuenta creada exitosamente"
        });
      }

      if (resultado.Mensaje == "Usuario ya existe en la Base de Datos.")
      {
        return Conflict(resultado); // 409 Conflict
      }

      return StatusCode(500, new ResponseMessages
      {
        Exitoso = false,
        Mensaje = "Ocurrió un error al crear la cuenta"
      });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] Login Usuarios)
    {
      //try
      {
        var resultado = await _usuarioServicio.LoginAsync(Usuarios.Correo, Usuarios.Contrasena);

        if (resultado.Exitoso)
        {
          return Ok(resultado);
        }

        if (resultado.Mensaje == "Credenciales inválidas")
        {
          return Conflict(resultado); // 409 Conflict
        }

        //return BadRequest(new ResultadoRegistro
        //{
        //  Exitoso = false,
        //  Mensaje = "No se pudo procesar la solicitud de inicio de sesión"
        //});
        //}

        //catch (Exception ex)
        {
          return StatusCode(500, new ResponseMessages
          {
            Exitoso = false,
            Mensaje = "Ocurrió un error inesperado en el servidor"
          });
        }
      }
    }

    [HttpGet("{idUsuario}/roles")]
    public async Task<IActionResult> ObtenerRolesPorUsuarioId(int idUsuario)
    {
      try
      {
        // Llama al servicio para obtener los roles del usuario
        var roles = await _usuarioServicio.ObtenerRolesPorUsuarioId(idUsuario);

        // Verifica si se obtuvieron resultados
        if (roles == null || roles.Count == 0)
        {
          return NotFound(new { mensaje = "No se encontraron roles para el usuario especificado." });
        }

        // Devuelve los roles en formato JSON
        return Ok(roles);
      }
      catch (Exception ex)
      {
        // Manejo de errores: devuelve un error 500 con el mensaje de excepción
        return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los roles del usuario.", detalle = ex.Message });
      }
    }
  }
}






