using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Aplicacion.Services.DTOS;

namespace JKC.Backend.Aplicacion.Services.UsuarioServices
{
  public interface IServicioUsuario
  {
    Task<Usuarios> ObtenerUsuarioPorId(int id);
    Task<List<Usuarios>> ObtenerTodosUsuarios();
    Task<ResponseMessages> RegistrarUsuarioAsync(Usuarios nuevoUsuario);
    Task<bool> ActualizarUsuarioAsync(Usuarios usuarioActualizado);
    Task<bool> EliminarUsuarioAsync(int id);
    Task<ResponseMessages> LoginAsync(string email, string password);

    Task<List<RolesUsuario>> ObtenerRolesPorUsuarioId(int idUsuario);

    // Yo obtendré permisos por Roles.
    //Task<List<RolesPermisos>> ObtenerPermisosPorUsuarioId(int idUsuario);
    //Task<List<RolesPermisos>> ObtenerPermisosPorRolId(int idRol);


  }
}
