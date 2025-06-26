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

    public async Task<Producto> ObtenerProductoPorId(int id)
    {
      return await _ProductoRepository.ObtenerPorId(id);
    }
    public async Task EliminarProductoPorId(int? id)
    {
      await _ProductoRepository.EliminarPorId(id);
    }

  }
}
