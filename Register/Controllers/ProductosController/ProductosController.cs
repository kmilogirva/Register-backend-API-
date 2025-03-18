using JKC.Backend.Aplicacion.Services.ProductosServices;
using JKC.Backend.Dominio.Entidades.Productos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.ProductosController
{
  [Route("api/[controller]")]
  [ApiController]
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


  }
}
