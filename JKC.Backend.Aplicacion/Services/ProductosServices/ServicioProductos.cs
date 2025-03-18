using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;


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


  }
}
