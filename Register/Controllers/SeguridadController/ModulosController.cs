using JKC.Backend.Aplicacion.Services.SeguridadService;
using JKC.Backend.Dominio.Entidades.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.SeguridadController
{
  //[Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class ModulosController : ControllerBase
  {
    private readonly IServicioModulo _servicioModulo;

    public ModulosController(IServicioModulo servicioModulo)
    {
      _servicioModulo = servicioModulo;
    }

    [HttpPost("crearmodulo")]
    public async Task<IActionResult> CrearModulo([FromBody] Modulo modulo)
    {
      var creado = await _servicioModulo.CrearModuloAsync(modulo);
      return Ok(creado);
    }

    [HttpGet("obtenermoduloporid/{id}")]
    public async Task<IActionResult> ObtenerModuloPorId(int id)
    {
      var modulo = await _servicioModulo.ObtenerPorIdAsync(id);
      if (modulo == null)
        return NotFound();

      return Ok(modulo);
    }

    [HttpGet("listarmodulos")]
    public async Task<IActionResult> ListarModulos()
    {
      var lista = await _servicioModulo.ObtenerTodosAsync();
      return Ok(lista);
    }

    [HttpPut("actualizarmodulo")]
    public async Task<IActionResult> ActualizarModulo([FromBody] Modulo modulo)
    {
      var actualizado = await _servicioModulo.ActualizarModuloAsync(modulo);
      return Ok(actualizado);
    }

    [HttpDelete("eliminarmodulo/{id}")]
    public async Task<IActionResult> EliminarModulo(int id)
    {
      var eliminado = await _servicioModulo.EliminarModuloAsync(id);
      return Ok(eliminado);
    }
  }

}

