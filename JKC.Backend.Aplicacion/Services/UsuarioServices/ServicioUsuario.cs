using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.DTO;

namespace JKC.Backend.Aplicacion.Services.UsuarioServices
{
  public class ServicioUsuario : IServicioUsuario
  {

    private readonly IRepository<Usuarios> _usuarioRepository;

    public ServicioUsuario(IRepository<Usuarios> usuarioRepository)
    {
      _usuarioRepository = usuarioRepository;
    }

    // Obtener usuario por Id
    public async Task<Usuarios> ObtenerUsuarioPorId(int id)
    {
      return await _usuarioRepository.ObtenerPorId(id);
    }

    public async Task<List<Usuarios>> ObtenerListadoUsuarios()
    {
      return await _usuarioRepository.ObtenerTodos().ToListAsync();
    }

    // Registrar un nuevo usuario
    public async Task<ResponseMessages> RegistrarUsuarioAsync(Usuarios nuevoUsuario)
    {

      var usuarioExistente = await _usuarioRepository
            .ObtenerTodos()
            .AnyAsync(u => u.Correo == nuevoUsuario.Correo);

      if (usuarioExistente)
      {
        return new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "Usuario ya existe en la Base de Datos."
        };
      }


      await _usuarioRepository.Crear(nuevoUsuario);

      return new ResponseMessages
      {
        Exitoso = true,
        Mensaje = "Usuario registrado exitosamente."
      };
    }

    // Actualizar un usuario existente
    public async Task<bool> ActualizarUsuario(Usuarios usuarioActualizado)
    {
      var usuarioExistente = await _usuarioRepository.ObtenerPorId(usuarioActualizado.IdUsuario);

      if (usuarioExistente == null)
        return false;

      // Actualiza los campos del usuario existente con los del actualizado
      usuarioExistente.Nombres = usuarioActualizado.Nombres;
      usuarioExistente.Apellidos = usuarioActualizado.Apellidos;
      usuarioExistente.Correo = usuarioActualizado.Correo;
      usuarioExistente.Telefono = usuarioActualizado.Telefono;
      usuarioExistente.Sexo = usuarioActualizado.Sexo;
      usuarioExistente.Contrasena = usuarioActualizado.Contrasena;


      await _usuarioRepository.Actualizar(usuarioExistente);
      return true;
    }

    // Eliminar un usuario por Id
    public async Task<bool> EliminarUsuarioPorId(int id)
    {
      var usuario = await _usuarioRepository.ObtenerPorId(id);

      if (usuario == null)
        return false;

      await _usuarioRepository.EliminarPorId(id);
      return true;
    }


    //public async Task<ResponseMessages> LoginAsync(string email, string password)
    //{
    //  // Busca al usuario con las credenciales proporcionadas
    //  var usuario = await _usuarioRepository.ObtenerTodos()
    //      .FirstOrDefaultAsync(u => u.Correo == email && u.Contrasena == password);

    //  // Si no se encuentra el usuario, devuelve un resultado fallido
    //  if (usuario == null)
    //  {
    //    return new ResponseMessages
    //    {
    //      Exitoso = false,
    //      Mensaje = "Credenciales inválidas"
    //    };
    //  }

    //  // Si el usuario se encuentra, devuelve un resultado exitoso
    //  return new ResponseMessages
    //  {
    //    Exitoso = true,
    //    Mensaje = "Inicio de sesión exitoso"
    //  };
    //}

    public async Task<List<RolesUsuario>> ObtenerRolesPorIdUsuario(int idUsuario)
    {
      try
      {

          var obtenerRolesUsuarios = await _usuarioRepository.EjecutarProcedimientoAlmacenado<RolesUsuario>("obtenerRolesUsuario", idUsuario);

          // Si se obtuvieron resultados, los devolvemos
          return obtenerRolesUsuarios.ToList();
      }
      catch (Exception ex)
      {
        // Manejo de excepciones: si ocurre algún error, lanzamos una nueva excepción
        throw new Exception("Error al obtener roles por usuario", ex);
      }
    }

    //public async Task<List<PermisoModuloDto>> ObtenerPermisosPorIdUsuario(int idUsuario)
    //{
    //  try
    //  {

    //    var permisos = await _usuarioRepository.EjecutarProcedimientoAlmacenado<PermisoModuloDto>("seguridad.obtenerPermisosxRolUsuario", idUsuario);


       
    //    return permisos.ToList();
    //  }
    //  catch (Exception ex)
    //  {
    //    // Manejo de excepciones: si ocurre algún error, lanzamos una nueva excepción
    //    throw new Exception("Error al obtener roles por usuario", ex);
    //  }
    //}



  }
}


