using JKC.Backend.Aplicacion.Services.ProductoServices;
using JKC.Backend.Dominio.Entidades.Producto.DTO;
using JKC.Backend.Dominio.Entidades.Productos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.ProductosController
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
 

  public class ProductosController : Controller
  {
    private readonly IServicioProducto _servicioProducto;

    public ProductosController(IServicioProducto servicioProducto)
    {
      _servicioProducto = servicioProducto;
    }

    [HttpPost("registrarproducto")]
    public async Task<IActionResult> RegistrarProducto([FromBody] Producto registroProductos)
    {
      if (registroProductos == null)
      {
        return BadRequest("Los datos del producto no pueden estar vacíos.");
      }

      if (!ModelState.IsValid)
      {
        return BadRequest("Datos del producto no son válidos.");
      }

      try
      {
        var resultado = await _servicioProducto.RegistrarProducto(registroProductos);
        return Ok(new { mensaje = "Producto registrado con éxito." });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Ocurrió un error al registrar el producto.", error = ex.Message });
      }
    }

    [HttpGet("listarproductos")]
    public async Task<IActionResult> ObtenerListadoProductos()
    {
      try
      {
        var resultado = await _servicioProducto.ObtenerListadoProductos();
        return Ok(resultado);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Ocurrió un error al obtener el listado de productos.");
      }
    }

    [HttpGet("obtenerproductoporid")]
    public async Task<IActionResult> ObtenerProductoPorId(int idProducto)
    {
      try
      {
        var producto = await _servicioProducto.ObtenerProductoPorId(idProducto);
        return Ok(producto);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Ocurrió un error al obtener los roles del usuario.");
      }
    }
       
    [HttpPut("actualizarproducto")]
    public async Task<IActionResult> ActualizarProducto(Producto producto)
    {
      //if (producto is null || idProducto != producto.IdProducto)
      //  return BadRequest(new { mensaje = "Datos inválidos." });

      try
      {
        var actualizado = await _servicioProducto.ActualizarProducto(producto);

        return actualizado
           ? Ok(new { mensaje = "Producto actualizado con éxito." })
           : NotFound(new { mensaje = "Producto no encontrado." });
      }
      catch (Exception ex)
      {
        return StatusCode(500,"Ocurrió un error al actualizar el producto.");
      }
    }


    //[HttpPost("eliminarproductosporids")]
    //public async Task<IActionResult> EliminarProductoAsync(List<int> idProductos)
    //{

    //  foreach (int Producto in idProductos)
    //  {
    //    var producto = await _servicioProducto.ObtenerProductoPorId(Producto);

    //    if (producto == null)
    //    {
    //      return NotFound(new { mensaje = "El producto no existe o ya ha sido eliminado." });
    //    }
    //    await _servicioProducto.EliminarProductoPorId(producto.IdProducto);
    //  }

    //  return Ok(new { mensaje = "Producto eliminado con éxito.", idProducto });
    //}

    //[HttpPost("eliminarproductosporids")]
    //public async Task<IActionResult> EliminarProductoAsync(List<int> idProductos)
    //{
    //  if (idProductos == null || !idProductos.Any())
    //    return BadRequest(new { mensaje = "No se enviaron productos para eliminar." });

    //  var productosEncontrados = new List<int>();
    //  var productosNoEncontrados = new List<int>();

    //  foreach (int id in idProductos)
    //  {
    //    var producto = await _servicioProducto.ObtenerProductoPorId(id);

    //    if (producto != null)
    //    {
    //      await _servicioProducto.EliminarProductoPorId(producto.IdProducto);
    //      productosEncontrados.Add(id);
    //    }
    //    else
    //    {
    //      productosNoEncontrados.Add(id);
    //    }
    //  }

    //  if (productosEncontrados.Any())
    //  {
    //    return Ok(new
    //    {
    //      mensaje = "Productos procesados.",
    //      productosEliminados = productosEncontrados,
    //      productosNoEncontrados = productosNoEncontrados
    //    });
    //  }
    //  else
    //  {
    //    return NotFound(new { mensaje = "Ningún producto encontrado para eliminar.", productosNoEncontrados });
    //  }
    //}

  }
}
