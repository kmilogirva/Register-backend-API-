using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.DTO;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{
  public class ServicioSeguridad: IServicioSeguridad
  {
    private readonly IRepository<Usuario> _usuarioRepository;

    public ServicioSeguridad(IRepository<Usuario> usuarioRepository)
    {
      _usuarioRepository = usuarioRepository;
    }

    public async Task<ResponseMessagesData<UsuarioDto>> LoginAsync(string email, string password)
    {
      if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
      {
        return new ResponseMessagesData<UsuarioDto>
        {
          Exitoso = false,
          Mensaje = "El correo o la contraseña no pueden estar vacíos.",
          Data = null
        };
      }

      // Busca al usuario con las credenciales proporcionadas
      var usuario = await _usuarioRepository.ObtenerTodos()
          .FirstOrDefaultAsync(u => u.Correo == email && u.Contrasena == password);

      // Si no se encuentra el usuario, devuelve un resultado fallido
      if (usuario == null)
      {
        return new ResponseMessagesData<UsuarioDto>
        {
          Exitoso = false,
          Mensaje = "Credenciales inválidas",
          Data = null
        };
      }

      // Si el usuario se encuentra, crea un objeto UsuarioDto con los datos necesarios
      var usuarioDto = new UsuarioDto
      {
        IdUsuario = usuario.IdUsuario,
        Nombre = usuario.Nombres,
        Correo = usuario.Correo
      };

      // Retorna el resultado exitoso con los datos del usuario
      return new ResponseMessagesData<UsuarioDto>
      {
        Exitoso = true,
        Mensaje = "Inicio de sesión exitoso",
        Data = usuarioDto
      };
    }



    //public async Task<List<Usuarios>> ObtenerListadoUsuario()
    //{
    //  return await _usuarioRepository.ObtenerTodos().ToListAsync();
    //}

    public async Task<List<PermisoModuloDto>> ObtenerPermisosPorIdUsuario(int idUsuario)
    {
      try
      {

        var permisos = await _usuarioRepository.EjecutarProcedimientoAlmacenado<PermisoModuloDto>("seguridad.obtenerPermisosxRolUsuario", idUsuario);
        return permisos.ToList();
      }
      catch (Exception ex)
      {
        // Manejo de excepciones: si ocurre algún error, lanzamos una nueva excepción
        throw new Exception("Error al obtener roles por usuario", ex);
      }
    }
  }
}
