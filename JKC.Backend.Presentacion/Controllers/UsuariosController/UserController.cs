using Jkc.Backend.Aplicacion.Services.EmailService;
using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Usuario;
using JKC.Backend.Dominio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace JKC.Backend.WebApi.Controllers.UsuariosController
{
  [ApiController]
  [Route("api/[controller]")]
  //[Authorize]
  public class UserController : ControllerBase
  {
    private readonly IServicioUsuario _usuarioServicio;
    private readonly IEmailService _emailService;


    public UserController(IServicioUsuario usuarioServicio, IEmailService emailService)
    {
      _usuarioServicio = usuarioServicio;
      _emailService = emailService;
    }

    [HttpGet("test")]
    public async Task<IActionResult> ProbarEnvio()
    {
      var destinatarios = new List<string>
    {
        "kmilogirva@gmail.com"
        //"rkevin943@gmail.com",
        //"pablcz0220@gmail.com",
        //"crhistandro@gmail.com"
    };
      //quiero guardar este archibo dentro de nuestra arquitectura y crear este metodo para traero de donde se crea
      // tabien quiero dos vistas una para olvidates contraseña y otra para el cambiodel contraseña el  link que envia al correo
      string mensajeHtml = @"
<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8'>
  <title>Correo de Prueba</title>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
  <table width='100%' cellpadding='0' cellspacing='0' style='background-color: #f4f4f4; padding: 20px 0;'>
    <tr>
      <td align='center'>
        <table width='600' cellpadding='0' cellspacing='0' style='background: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
          <!-- Encabezado -->
          <tr>
            <td align='center' style='background-color: #2C3E50; padding: 20px;'>
              <h1 style='color: #ffffff; margin: 0; font-size: 24px;'>JKC Inventory</h1>
            </td>
          </tr>
          <!-- Contenido -->
          <tr>
            <td style='padding: 30px; color: #333333; font-size: 16px; line-height: 1.5;'>
              <p>Estimado(a),</p>
              <p>Este es un <strong>correo de prueba</strong> enviado desde <b>C#</b>.</p>
              <p style='margin-top: 20px;'>🚀 ¡Todo funciona correctamente!</p>
            </td>
          </tr>
          <!-- Footer -->
          <tr>
            <td align='center' style='background-color: #ecf0f1; padding: 15px; font-size: 12px; color: #7f8c8d;'>
              © 2025 JKC Inventory. Todos los derechos reservados.
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";


      bool enviado = await _emailService.EnviarEmailAsync(
          destinatarios,
          "Prueba de correo enviado desde JKCInventory Enviado desde C# 🚀",
          mensajeHtml
      );

      return enviado ? Ok("Correo enviado correctamente") : StatusCode(500, "Error al enviar el correo");
    }

    // POST: api/User/Create
    [HttpPost("crearusuario")]
    public async Task<IActionResult> CreateUsers([FromBody] Usuario nuevoUsuario)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest("Datos inválidos");
      }

      //nuevoUsuario.FechaCreacion = DateTime.Now;

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
        return Conflict(resultado); 
      }

      return StatusCode(500, new ResponseMessages
      {
        Exitoso = false,
        Mensaje = "Ocurrió un error al crear la cuenta"
      });
    }

    [HttpGet("obtenerusuarioporid")]
    public async Task<IActionResult> ObtenerUsuarioPorId(int idUsuario)
    {
      try
      {
        var usuario = await _usuarioServicio.ObtenerUsuarioPorId(idUsuario);
        return Ok(usuario);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Ocurrió un error al obtener los roles del usuario.");
      }
    }

    [HttpPut("actualizarusuario")]
    public async Task<IActionResult> ActualizarUsuarioAsync(Usuario nuevousuario)
    {
      //var usuarioexistente = await _usuarioServicio.ObtenerUsuarioPorId(nuevousuario.IdUsuario);

      //if (usuarioexistente == null)
      //{
      //  return NotFound(new { mensaje = "El usuario no existe" });
      //}

      await _usuarioServicio.ActualizarUsuario(nuevousuario);
      return Ok(new { mensaje = "El usuario ha sido actualizado con éxito.", nuevousuario.IdUsuario });
    }

    [HttpGet("listarusuarios")]
    public async Task<IActionResult> ObtenerListadoProductos()
    {
      try
      {
        var resultado = await _usuarioServicio.ObtenerListadoUsuarios();
        return Ok(resultado);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Ocurrió un error al obtener el listado de productos.");
      }
    }

    [HttpDelete("eliminarusuarioporid")]
    public async Task<IActionResult> EliminarUsuariooAsync(int idUsuario)
    {
      var usuario = await _usuarioServicio.ObtenerUsuarioPorId(idUsuario);

      if (usuario == null)
      {
        return NotFound(new { mensaje = "El usuario no existe o ya ha sido eliminado." });
      }

      await _usuarioServicio.EliminarUsuarioPorId(idUsuario);
      return Ok(new { mensaje = "El usuario ha sido eliminado con éxito.", idUsuario });
    }


    //[HttpGet("consultarrolesporusuario/{idUsuario}")]
    //public async Task<IActionResult> ObtenerRolesPorIdUsuario(int idUsuario)
    //{
    //  try
    //  {
    //    var roles = await _usuarioServicio.ObtenerRolesPorIdUsuario(idUsuario);

    //    if (roles == null || roles.Count == 0)
    //    {
    //      return NotFound(new { mensaje = "No se encontraron roles para el usuario especificado." });
    //    }

    //    return Ok(roles);
    //  }
    //  catch (Exception ex)
    //  {
    //    return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los roles del usuario.", detalle = ex.Message });
    //  }
    //}


    [HttpGet("consultarusuarios")]
    public async Task<IActionResult> ObtenerUsuariosResponse()
    {
      try
      {
        var usuarios = await _usuarioServicio.ObtenerUsuariosResponse();

        if (usuarios == null || usuarios.Count == 0)
        {
          return NotFound(new { mensaje = "No se encontraron roles para el usuario especificado." });
        }

        return Ok(usuarios);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los roles del usuario.", detalle = ex.Message });
      }
    }

    [HttpGet("consultarusuariosporidtercero/{idTercero}")]
    public async Task<IActionResult> ObtenerUsuariosPorIdTercero(int idTercero)
    {
      try
      {
        var usuario = await _usuarioServicio.ObtenerUsuarioPorIdTercero(idTercero);

        return Ok(usuario);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los roles del usuario.", detalle = ex.Message });
      }
    }



  }
}
