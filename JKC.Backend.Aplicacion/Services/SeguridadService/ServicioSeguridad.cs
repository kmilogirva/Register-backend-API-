using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.DTO;
using JKC.Backend.Dominio.Entidades.Usuario;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using System.Linq;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{
  public class ServicioSeguridad : IServicioSeguridad
  {
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IRepository<Roles> _rolesRepository;

    public ServicioSeguridad(IRepository<Usuario> usuarioRepository, IRepository<Roles> rolesRepository)
    {
      _usuarioRepository = usuarioRepository;
      _rolesRepository = rolesRepository;
    }

    public async Task<Roles> CrearRol(Roles nuevoRol)
    {
      try
      {
        if (nuevoRol == null)
          return null;

        nuevoRol.NombreRol = nuevoRol.NombreRol.Trim();
        if (string.IsNullOrEmpty(nuevoRol.NombreRol))
          throw new ArgumentException("El nombre del rol no puede estar vacío.");

        bool existeRol = await _rolesRepository.AnyAsync(
          r => r.NombreRol.ToLower() == nuevoRol.NombreRol.ToLower());

        if (existeRol)
          return null;

        nuevoRol.FechaCreacion = DateTime.UtcNow;

        await _rolesRepository.Crear(nuevoRol);
        return nuevoRol;
      }
      catch (Exception ex)
      {
        throw new Exception("Error al crear el rol", ex);
      }
    }

    public async Task<List<Roles>> ObtenerTodosRoles()
    {
      var roles = await _rolesRepository.ObtenerTodos();
      return roles.ToList();
    }

    // Este método se llama cuando Angular hace PUT /api/seguridad/roles/{id}
    public async Task<Roles> ActualizarRol(int id, Roles rolActualizado)
    {
      var rolExistente = await _rolesRepository.ObtenerPorId(id);

      if (rolExistente == null)
        return null;

      // Actualizamos los datos
      rolExistente.NombreRol = rolActualizado.NombreRol;
      rolExistente.IdEstado = rolActualizado.IdEstado;
      rolExistente.FechaModificacion = DateTime.UtcNow;
      rolExistente.IdUsuarioModificacion = rolActualizado.IdUsuarioModificacion;

      await _rolesRepository.Actualizar(rolExistente);

      return rolExistente;
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


      // Busca al usuario con las credenciales proporcionadas y valida si se encuantra activo
      var usuario = await _usuarioRepository.ObtenerTodos();

      var usuarioAutorizado = usuario.FirstOrDefault(u =>
        u.Correo == email &&
        u.Contrasena == password &&
        u.IdEstado == 1);

      // Si no se encuentra el usuario, devuelve un resultado fallido
      if (usuarioAutorizado == null)

      {
        return new ResponseMessagesData<UsuarioDto>
        {
          Exitoso = false,
          Mensaje = "Credenciales inválidas",
          Data = null
        };
      }

      var usuarioDto = new UsuarioDto
      {
        IdUsuario = usuarioAutorizado.IdUsuario,
        Nombre = usuarioAutorizado.NombreCompleto,
        //Nombre = string.Join(" ", new[] {
        //    usuarioAutorizado.Nombre1,
        //    usuarioAutorizado.Nombre2,
        //    usuarioAutorizado.Apellido1,
        //    usuarioAutorizado.Apellido2
        //}.Where(s => !string.IsNullOrWhiteSpace(s))),
        Correo = usuarioAutorizado.Correo,
        IdRol = usuarioAutorizado.IdRol
      };

      return new ResponseMessagesData<UsuarioDto>
      {
        Exitoso = true,
        Mensaje = "Inicio de sesión exitoso",
        Data = usuarioDto
      };
    }

    public async Task<List<Roles>> ObtenerListadoRoles()
    {
      return await _rolesRepository.ObtenerTodos();
    }



    //public async Task<List<Usuarios>> ObtenerListadoUsuario()
    //{
    //  return await _usuarioRepository.ObtenerTodos().ToListAsync();
    //}

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
