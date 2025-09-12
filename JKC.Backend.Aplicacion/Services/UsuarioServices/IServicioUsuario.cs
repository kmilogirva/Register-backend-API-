using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Dominio.Entidades.Response.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.producto;
using JKC.Backend.Dominio.Entidades.Usuario;

namespace JKC.Backend.Aplicacion.Services.UsuarioServices
{
  public interface IServicioUsuario
  {
    Task<Usuario> ObtenerUsuarioPorId(int id);
    //Task<Usuario> ObtenerUsuarioPorCorreo(string correo); // <- NUEVO
    Task<List<Usuario>> ObtenerListadoUsuarios();
    Task<List<UsuarioResponse>> ObtenerUsuariosResponse();
    Task<UsuarioResponse> ObtenerUsuarioPorIdTercero(int idTercero);
    Task<ResponseMessages> RegistrarUsuarioAsync(Usuario nuevoUsuario);
    Task<bool> ActualizarUsuario(Usuario usuarioActualizado);
    Task<bool> EliminarUsuarioPorId(int id);
    Task<(ResponseMessages Respuesta, string Token)> SolicitarRecuperacionContrasenaAsync(string correo);


    Task<ResponseMessages> RestablecerContrasenaAsync(string token, string nuevaContrasena); // <- NUEVO

    Task<List<RolesUsuario>> ObtenerRolesPorIdUsuario(int idUsuario);
    Task<bool> ValidarTokenAsync(string token);
    
  }
}

//Task<List<PermisoModuloproducto>> ObtenerPermisosPorIdUsuario(int idUsuario);

// Yo obtendré permisos por Roles.
//Task<List<RolesPermisos>> ObtenerPermisosPorUsuarioId(int idUsuario);
//Task<List<RolesPermisos>> ObtenerPermisosPorRolId(int idRol);



