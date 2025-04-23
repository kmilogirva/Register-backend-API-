using JKC.Backend.Aplicacion.Services.ProductosServices;
using JKC.Backend.Dominio.Entidades.Productos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.ProductosController
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
 

  public class ProductosController : Controller
  {
    private readonly IServicioProductos _servicioProductos;

    public ProductosController(IServicioProductos servicioProductos)
    {
      _servicioProductos = servicioProductos;
    }

    [HttpPost("registrarproducto")]
    public async Task<IActionResult> RegistrarProducto([FromBody] Productos registroProductos)
    {
      if (registroProductos == null)
      {
        return BadRequest(new { mensaje = "Los datos del producto no pueden estar vacíos." });
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(new { mensaje = "Datos del producto no son válidos.", errores = ModelState.Values.SelectMany(v => v.Errors) });
      }

      try
      {
        var resultado = await _servicioProductos.RegistrarProducto(registroProductos);
        return Ok(new { mensaje = "Producto registrado con éxito.", producto = resultado });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al registrar el producto.", error = ex.Message });
      }
    }

    [HttpPost("listarproductos")]
    public async Task<IActionResult> ObtenerListadoProductos()
    {
      try
      {
        var resultado = await _servicioProductos.ObtenerListadoProductos();
        return Ok(new { mensaje = "Listado de productos obtenido con éxito.", productos = resultado });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al obtener el listado de productos.", error = ex.Message });
      }
    }

    [HttpGet("obtenerproductoporid")]
    public async Task<IActionResult> ObtenerRolesPorUsuarioId(int idProducto)
    {
      try
      {
        var producto = await _servicioProductos.ObtenerProductoPorId(idProducto);
        return Ok(producto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los roles del usuario.", detalle = ex.Message });
      }
    }

    [HttpPost("eliminarproductoporid")]
    public async Task<IActionResult> EliminarProductoAsync(int idProducto)
    {
      var producto = await _servicioProductos.ObtenerProductoPorId(idProducto);

      if (producto == null)
      {
        return NotFound(new { mensaje = "El producto no existe o ya ha sido eliminado." });
      }

      await _servicioProductos.EliminarProductoPorId(idProducto);
      return Ok(new { mensaje = "Producto eliminado con éxito.", idProducto });
    }



  }
}
