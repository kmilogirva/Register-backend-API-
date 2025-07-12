using System.Collections.Generic;
using System.Threading.Tasks;
using JKC.Backend.Aplicacion.Services.BodegaServices;
using JKC.Backend.Dominio.Entidades;
using JKC.Backend.Dominio.Entidades.Bodegas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.Aplicacion.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class BodegasController : ControllerBase
  {
    private readonly IServicioBodega _servicioBodega;

    public BodegasController(IServicioBodega servicioBodega)
    {
      _servicioBodega = servicioBodega;
    }

    [HttpPost("listarbodegas")]
    public async Task<IActionResult> ObtenerListadoCategorias()
    {
      try
      {
        var resultado = await _servicioBodega.ObtenerListadoBodegas();
        return Ok(new { mensaje = "Listado de categorias obtenido con éxito.", productos = resultado });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al obtener el listado de productos.", error = ex.Message });
      }
    }
    ////[HttpGet("ObtenerTodaslasBodegas")]
    //public async Task<ActionResult<IEnumerable<Bodega>>> GetBodegas()
    //{
    //  var bodegas = await _servicioBodega.ObtenerListadoBodegas();
    //  return Ok(bodegas);
    //}

    [HttpGet("BuscarBodegaporId")]
    public async Task<ActionResult<Bodega>> GetBodega(int id)
    {
      var bodega = await _servicioBodega.ObtenerBodegaPorId(id);

      if (bodega == null)
      {
        return NotFound();
      }

      return Ok(bodega);
    }

    [HttpPost("CrearBodega")]
    public async Task<ActionResult<Bodega>> PostBodega([FromBody] Bodega bodega)
    {
      var nuevaBodega = await _servicioBodega.RegistrarBodega(bodega);
      return CreatedAtAction(nameof(GetBodega), new { id = nuevaBodega.IdBodega }, nuevaBodega);
    }

    [HttpPut("ActualizaunaBodegaporid")]
    public async Task<IActionResult> PutBodega(int id, [FromBody] Bodega bodega)
    {
      if (id != bodega.IdBodega)
      {
        return BadRequest();
      }

      // Opcional: en servicios avanzados se puede implementar una lógica Update específica
      // Aquí simulamos una actualización eliminando y volviendo a crear
      await _servicioBodega.EliminarBodegaPorId(id);
      await _servicioBodega.RegistrarBodega(bodega);

      return NoContent();
    }

    [HttpDelete("EliminarbodegaporId")]
    public async Task<IActionResult> DeleteBodega(int id)
    {
      var bodega = await _servicioBodega.ObtenerBodegaPorId(id);
      if (bodega == null)
      {
        return NotFound();
      }

      await _servicioBodega.EliminarBodegaPorId(id);
      return NoContent();
    }
  }
}
