using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Usuario;
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

    public UserController(IServicioUsuario usuarioServicio)
    {
      _usuarioServicio = usuarioServicio;
    }

    // POST: api/User/Create
    [HttpPost("crearusuario")]
    public async Task<IActionResult> CreateUsers([FromBody] Usuario nuevoUsuario)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest("Datos inválidos");
      }

      nuevoUsuario.FechaCreacion = DateTime.Now;

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
      var usuarioexistente = await _usuarioServicio.ObtenerUsuarioPorId(nuevousuario.IdUsuario);

      if (usuarioexistente == null)
      {
        return NotFound(new { mensaje = "El usuario no existe" });
      }

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

    

  }
}






