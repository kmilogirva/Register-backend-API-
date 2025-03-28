using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;


namespace JKC.Backend.Aplicacion.Services.ProductosServices
{
  public class ServicioProductos : IServicioProductos
  {

    private readonly IRepository<Productos> _productosRepository;

    public ServicioProductos(IRepository<Productos> productosRepository)
    {
      _productosRepository = productosRepository;
    }

    public async Task<Productos> RegistrarProducto(Productos registroProductos)
    {
      await _productosRepository.Crear(registroProductos);
      return registroProductos;
    }

    public async Task<List<Productos>> ObtenerListadoProductos()
    {
      return await _productosRepository.ObtenerTodos().ToListAsync();
    }

    public async Task<Productos> ObtenerProductoPorId(int id)
    {
      return await _productosRepository.ObtenerPorId(id);
    }

    public async Task EliminarProductoPorId(int id)
    {
      await _productosRepository.EliminarPorId(id);
    }

  }
}
