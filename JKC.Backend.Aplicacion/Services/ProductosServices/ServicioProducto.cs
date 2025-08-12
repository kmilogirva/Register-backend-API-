using JKC.Backend.Dominio.Entidades.Producto.DTO;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;


namespace JKC.Backend.Aplicacion.Services.ProductoServices
{
  public class ServicioProducto : IServicioProducto
  {

    private readonly IRepository<Producto> _ProductoRepository;

    public ServicioProducto(IRepository<Producto> ProductoRepository)
    {
      _ProductoRepository = ProductoRepository;
    }

    public async Task<Producto> RegistrarProducto(Producto registroProducto)
    {
      await _ProductoRepository.Crear(registroProducto);
      return registroProducto;
    }

    public async Task<List<Producto>> ObtenerListadoProductos()
    {
      return await _ProductoRepository.ObtenerTodos();
    }

    public async Task<Producto> ObtenerProductoPorId(int? id)
    {
      return await _ProductoRepository.ObtenerPorId(id);
    }

    public async Task<bool> ActualizarProducto(Producto producto)
    {
      var productoExistente = await _ProductoRepository.ObtenerPorId(producto.IdProducto);
      if (productoExistente is null)
        return false;


      productoExistente.FechaModificacion = DateTime.UtcNow;
      productoExistente.IdUsuarioModificacion = producto.IdUsuarioModificacion;
      productoExistente.CodEan = producto.CodEan.Trim();
      productoExistente.NomProducto = producto.NomProducto.Trim();
      productoExistente.IdCategoria = producto.IdCategoria;
      productoExistente.UbicacionProducto = producto.UbicacionProducto.Trim();
      productoExistente.Cantidad = producto.Cantidad;
      productoExistente.Observacion = producto.Observacion?.Trim();

      await _ProductoRepository.Actualizar(productoExistente);
      return true;
    }

    public async Task EliminarProductoPorId(int? id)
    {
      await _ProductoRepository.EliminarPorId(id);
    }

  }
}
