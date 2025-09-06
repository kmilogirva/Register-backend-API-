using JKC.Backend.Aplicacion.Services.CategoriasServices;
using JKC.Backend.Dominio.Entidades.Categorias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.CategoriasController
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class CategoriasController : Controller
  {
    private readonly IServicioCategoria _servicioCategoria;

    public CategoriasController(IServicioCategoria servicioCategoria)
    {
      _servicioCategoria = servicioCategoria;
    }

    [HttpGet("listarcategorias")]
    public async Task<ActionResult<List<Categoria>>> ObtenerListadoCategorias()
    {
      var resultado = await _servicioCategoria.ObtenerListadoCategorias();
      return Ok(resultado);
    }

    [HttpGet("obtenercategoriaporid/{id}")]
    public async Task<ActionResult<Categoria>> ObtenerCategoriaPorId(int id)
    {
      var categoria = await _servicioCategoria.ObtenerCategoriaPorId(id);
      if (categoria == null)
      {
        return NotFound(new { mensaje = $"No se encontró la categoría con ID {id}." });
      }
      return Ok(categoria);
    }

    [HttpPost("registrarcategoria")]
    public async Task<ActionResult<Categoria>> RegistrarCategoria([FromBody] Categoria registroCategoria)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var resultado = await _servicioCategoria.RegistrarCategoria(registroCategoria);
      return CreatedAtAction(nameof(ObtenerCategoriaPorId), new { id = resultado.IdCategoria }, resultado);
    }

    [HttpPut("actualizarcategoria/{id}")]
    public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] Categoria categoriaActualizada)
    {
      if (id != categoriaActualizada.IdCategoria)
      {
        return BadRequest();
      }
      await _servicioCategoria.ActualizarCategoria(categoriaActualizada);
      return NoContent();
    }

    [HttpDelete("eliminarcategoriasporids/{id}")]
    public async Task<IActionResult> EliminarCategoria(int id)
    {
      var categoriaExistente = await _servicioCategoria.ObtenerCategoriaPorId(id);
      if (categoriaExistente == null)
      {
        return NotFound();
      }
      await _servicioCategoria.EliminarCategoriasPorIds(id);
      return NoContent();
    }
  }
}
