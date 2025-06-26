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
    //Task<Usuarios> ObtenerUsuarioPorId(int id);
    //Task<List<Usuarios>> ObtenerTodosUsuarios();

    Task<Producto> RegistrarProducto(Producto registroProducto);
    Task<List<Producto>> ObtenerListadoProductos();
    Task<Producto> ObtenerProductoPorId(int id);
    Task EliminarProductoPorId(int? id);
    //Task<ResultadoRegistro> RegistrarUsuarioAsync(Usuarios nuevoUsuario);
    //Task<bool> ActualizarUsuarioAsync(Usuarios usuarioActualizado);
    //Task<bool> EliminarUsuarioAsync(int id);
    //Task<ResultadoRegistro> LoginAsync(string email, string password);

    //Task<List<RolesUsuario>> ObtenerRolesPorUsuarioId(int idUsuario);
  }
}
