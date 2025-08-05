using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.DTO;
using JKC.Backend.Dominio.Entidades.Usuario;

namespace JKC.Backend.Aplicacion.Services.UsuarioServices
{
  public interface IServicioUsuario
  {
    Task<Usuario> ObtenerUsuarioPorId(int id);
    Task<List<Usuario>> ObtenerListadoUsuarios();
    Task<ResponseMessages> RegistrarUsuarioAsync(Usuario nuevoUsuario);
    Task<bool> ActualizarUsuario(Usuario usuarioActualizado);
    Task<bool> EliminarUsuarioPorId(int id);
    //Task<ResponseMessages> LoginAsync(string email, string password);

    Task<List<RolesUsuario>> ObtenerRolesPorIdUsuario(int idUsuario);

    //Task<List<PermisoModuloDto>> ObtenerPermisosPorIdUsuario(int idUsuario);

    // Yo obtendré permisos por Roles.
    //Task<List<RolesPermisos>> ObtenerPermisosPorUsuarioId(int idUsuario);
    //Task<List<RolesPermisos>> ObtenerPermisosPorRolId(int idRol);


  }
}
