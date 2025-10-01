using JKC.Backend.Aplicacion.Services.MovimientoServices;
using JKC.Backend.Dominio.Entidades.Movimientos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.Aplicacion.Controllers
{
  // [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class MovimientosController : ControllerBase
  {
    private readonly IServicioMovimiento _servicioMovimiento;

    public MovimientosController(IServicioMovimiento servicioMovimiento)
    {
      _servicioMovimiento = servicioMovimiento;
    }

    [HttpGet("listarmovimientos")]
    public async Task<IActionResult> ObtenerListadoMovimientos()
    {
      try
      {
        var resultado = await _servicioMovimiento.ObtenerListadoMovimientos();
        return Ok(resultado);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los movimientos.", error = ex.Message });
      }
    }

    // Alineado: recibir id en query como 'id' o 'idMovimiento' según tu cliente
    [HttpGet("obtenermovimientoporid")]
    public async Task<ActionResult<Movimiento>> GetMovimiento([FromQuery] int id)
    {
      var movimiento = await _servicioMovimiento.ObtenerMovimientoPorId(id);

      if (movimiento == null)
      {
        return NotFound();
      }

      return Ok(movimiento);
    }

    [HttpPost("CrearMovimiento")]
    public async Task<ActionResult<Movimiento>> PostMovimiento([FromBody] Movimiento movimiento)
    {
      try
      {
        var movimientos = new Movimiento
        {
          IdProducto = movimiento.IdProducto,
          IdTipoMovimiento = movimiento.IdTipoMovimiento,
          Cantidad = movimiento.Cantidad,
          Observacion = movimiento.Observacion,
          IdUsuarioCreacion = movimiento.IdUsuarioCreacion,
          FechaCreacion = DateTime.UtcNow
        };

        var nuevoMovimiento = await _servicioMovimiento.RegistrarMovimiento(movimientos);
        return Ok(nuevoMovimiento);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al registrar el movimiento.", error = ex.Message });
      }
    }


    // PUT: recibimos todo en el body; el cliente NO envía id en URL
    [HttpPut("actualizarmovimiento")]
    public async Task<IActionResult> PutMovimiento([FromBody] Movimiento movimiento)
    {
      if (movimiento == null || movimiento.IdMovimiento == 0)
        return BadRequest("IdMovimiento inválido.");

      try
      {
        await _servicioMovimiento.ActualizarMovimiento(movimiento);
        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al actualizar el movimiento.", error = ex.Message });
      }
    }

    // DELETE: aceptar id en ruta, para que encaje con DELETE /.../eliminarmovimientoporid/{id}
    [HttpDelete("eliminarmovimientoporid/{id}")]
    public async Task<IActionResult> DeleteMovimiento(int id)
    {
      var movimiento = await _servicioMovimiento.ObtenerMovimientoPorId(id);
      if (movimiento == null)
      {
        return NotFound();
      }

      await _servicioMovimiento.EliminarMovimientoPorId(id);
      return NoContent();
    }
  }
}
