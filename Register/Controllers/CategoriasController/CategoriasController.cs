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

    [HttpPost("registrarcategoria")]
    public async Task<IActionResult> RegistrarCategoria([FromBody] Categoria registroCategoria)
    {
      if (registroCategoria == null)
      {
        return BadRequest(new { mensaje = "Los datos de la categoria no pueden estar vacíos." });
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(new { mensaje = "Datos de la categoria no son válidos.", errores = ModelState.Values.SelectMany(v => v.Errors) });
      }

      try
      {
        var resultado = await _servicioCategoria.RegistrarCategoria(registroCategoria);
        return Ok(new { mensaje = "Categoria registrada con éxito.", producto = resultado });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al registrar la categoria", error = ex.Message });
      }
    }

    [HttpPost("listarcategorias")]
    public async Task<IActionResult> ObtenerListadoCategorias()
    {
      try
      {
        var resultado = await _servicioCategoria.ObtenerListadoCategorias();
        return Ok(new { mensaje = "Listado de categorias obtenido con éxito.", productos = resultado });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al obtener el listado de productos.", error = ex.Message });
      }
    }

    [HttpGet("obtenercategoriaporid")]
    public async Task<IActionResult> ObtenerRolesPorUsuarioId(int idCategoria)
    {
      try
      {
        var categoria = await _servicioCategoria.ObtenerCategoriaPorId(idCategoria);
        return Ok(categoria);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los roles del usuario.", detalle = ex.Message });
      }
    }

    [HttpPost("eliminarcategoriasporids")]
    public async Task<IActionResult> EliminarCategoriaAsync(List<int> idCategoria)
    {
      if (idCategoria == null || !idCategoria.Any())
        return BadRequest(new { mensaje = "No se enviaron productos para eliminar." });

      var categoriasEncontradas = new List<int>();
      var categoriasNoEncontradas = new List<int>();

      foreach (int id in idCategoria)
      {
        var producto = await _servicioCategoria.ObtenerCategoriaPorId(id);

        if (producto != null)
        {
          await _servicioCategoria.EliminarCategoriasPorIds(id);
          categoriasEncontradas.Add(id);
        }
        else
        {
          categoriasNoEncontradas.Add(id);
        }
      }

      if (categoriasEncontradas.Any())
      {
        return Ok(new
        {
          mensaje = "Productos procesados.",
          categoriasEliminadas = categoriasEncontradas,
          categoriasNoEncontradas = categoriasNoEncontradas
        });
      }
      else
      {
        return NotFound(new { mensaje = "Ningún producto encontrado para eliminar.", categoriasNoEncontradas });
      }
    }
  }
}
