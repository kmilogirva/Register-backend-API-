using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.DTO;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{

  public interface IServicioSeguridad
  {
    Task<ResponseMessagesData<UsuarioDto>> LoginAsync(string email, string password);

    Task<Roles> CrearRol(Roles nuevoRol);
    Task<List<Roles>> ObtenerTodosRoles();
    Task<List<Roles>> ObtenerListadoRoles();
    Task<List<RolPermisosAccionDTO>> ObtenerPermisosPorRol(int idRol);
    Task<Roles> ActualizarRol(int id, Roles rolActualizado);
    Task<bool> EliminarRol(int id);
    Task<List<RolPermisosAccionDTO>> CrearPermisosRolesAcciones(List<AsignarPermisos> asignarPermisosLista);
    Task<bool> EliminarPermisosPorRol(int idRol);

  }
}
