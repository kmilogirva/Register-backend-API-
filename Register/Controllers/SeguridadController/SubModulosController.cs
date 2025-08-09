using JKC.Backend.Aplicacion.Services.SeguridadService;
using JKC.Backend.Dominio.Entidades.Seguridad;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.SeguridadController
{
  [ApiController]
  [Route("api/[controller]")]
  public class SubModulosController : ControllerBase
  {
      private readonly IServicioSubModulo _servicioSubmodulo;

      public SubModulosController(IServicioSubModulo servicioSubmodulo)
      {
        this._servicioSubmodulo = servicioSubmodulo;
      }

      [HttpPost("crearsubmodulo")]
      public async Task<IActionResult> CrearSubmodulo([FromBody] SubModulo submodulo)
      {
        var creado = await _servicioSubmodulo.CrearSubmodulo(submodulo);
        return Ok(creado);
      }

      [HttpGet("obtenersubmoduloporid/{id}")]
      public async Task<IActionResult> ObtenerPorId(int id)
      {
        var submodulo = await _servicioSubmodulo.ObtenerPorId(id);
        if (submodulo == null) return NotFound();
        return Ok(submodulo);
      }

      [HttpGet("listarsubmodulos")]
      public async Task<IActionResult> Listar()
      {
        var lista = await _servicioSubmodulo.ObtenerTodosInclude();
        return Ok(lista);
      }

      [HttpPut("actualizarsubmodulo")]
      public async Task<IActionResult> Actualizar([FromBody] SubModulo submodulo)
      {
        var actualizado = await _servicioSubmodulo.ActualizarSubmodulo(submodulo);
        return Ok(actualizado);
      }

      [HttpDelete("eliminarsubmodulo/{id}")]
      public async Task<IActionResult> Eliminar(int id)
      {
        var eliminado = await _servicioSubmodulo.EliminarSubmodulo(id);
        return Ok(eliminado);
      }
    }
  }

