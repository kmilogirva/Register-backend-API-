using JKC.Backend.Dominio.Entidades.Producto.DTO;
using JKC.Backend.Dominio.Entidades.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.ProductoServices
{
  public interface IServicioProducto
  {

    Task<Producto> RegistrarProducto(Producto registroProducto);
    Task<List<Producto>> ObtenerListadoProductos();
    Task<Producto> ObtenerProductoPorId(int? id);
    Task<bool> EliminarProductoPorId(int id);

    Task<bool> ActualizarProducto(Producto productoActualizado);
    //Task<bool> EliminarUsuarioAsync(int id);
    //Task<ResultadoRegistro> LoginAsync(string email, string password);

    //Task<List<RolesUsuario>> ObtenerRolesPorUsuarioId(int idUsuario);
  }
}
