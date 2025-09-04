using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.GeneralesServices;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.GeneralesController
{
  [ApiController]
  [Route("api/[controller]")]
  public class TercerosController : Controller
  {
    private readonly IServicioTercero _terceroServicio;

    public TercerosController(IServicioTercero terceroServicio)
    {
      _terceroServicio = terceroServicio;
    }

    [HttpPost("creartercero")]
    public async Task<IActionResult> CreateUsers([FromBody] Tercero nuevoTercero)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest("Datos inválidos");
      }

      nuevoTercero.FechaCreacion = DateTime.Now;

      var resultado = await _terceroServicio.RegistrarTercero(nuevoTercero);

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

    [HttpGet("obtenerterceroporid")]
    public async Task<IActionResult> ObtenerUsuarioPorId(int idUsuario)
    {
      try
      {
        var usuario = await _terceroServicio.ObtenerTerceroPorId(idUsuario);
        return Ok(usuario);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Ocurrió un error al obtener los roles del usuario.");
      }
    }

    [HttpPut("actualizartercero")]
    public async Task<IActionResult> ActualizarUsuarioAsync(Tercero nuevotercero)
    {
      //var usuarioexistente = await _terceroServicio.ObtenerTerceroPorId(nuevotercero.IdTercero);

      //if (usuarioexistente == null)
      //{
      //  return NotFound(new { mensaje = "El usuario no existe" });
      //}

      await _terceroServicio.ActualizarTercero(nuevotercero);
      return Ok(new { mensaje = "El usuario ha sido actualizado con éxito.", nuevotercero.IdTercero });
    }

    [HttpGet("listarterceros")]
    public async Task<IActionResult> ObtenerListadoProductos()
    {
      try
      {
        var resultado = await _terceroServicio.ObtenerListadoTerceros();
        return Ok(resultado);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Ocurrió un error al obtener el listado de productos.");
      }
    }

    [HttpDelete("eliminarterceroporid")]
    public async Task<IActionResult> EliminarUsuariooAsync(int idUsuario)
    {
      var usuario = await _terceroServicio.ObtenerTerceroPorId(idUsuario);

      if (usuario == null)
      {
        return NotFound(new { mensaje = "El usuario no existe o ya ha sido eliminado." });
      }

      await _terceroServicio.EliminarTerceroPorId(idUsuario);
      return Ok(new { mensaje = "El usuario ha sido eliminado con éxito.", idUsuario });
    }
  }
}
