using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.DTO;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{

  public interface IServicioSeguridad
  {
    //Task<ResponseMessages> LoginAsync(string email, string password);
    Task<ResponseMessagesData<UsuarioDto>> LoginAsync(string email, string password);
    Task<List<PermisoModuloDto>> ObtenerPermisosPorIdUsuario(int idUsuario);
  }
}
