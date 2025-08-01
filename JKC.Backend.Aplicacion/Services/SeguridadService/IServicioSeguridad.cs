using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad;
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
    //Task<List<PermisoModuloDto>> ObtenerPermisosPorIdUsuario(int idUsuario);
    Task<Roles> CrearRol(Roles nuevoRol);
    Task<List<Roles>> ObtenerListadoRoles();
  }
}
