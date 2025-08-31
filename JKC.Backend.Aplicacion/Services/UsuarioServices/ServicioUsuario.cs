using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Dominio.Entidades.Response.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.producto;
using JKC.Backend.Dominio.Entidades.Usuario;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace JKC.Backend.Aplicacion.Services.UsuarioServices
{
  public class ServicioUsuario : IServicioUsuario
  {

    private readonly IRepository<Usuario> _usuarioRepository;

    public ServicioUsuario(IRepository<Usuario> usuarioRepository)
    {
      _usuarioRepository = usuarioRepository;
    }

    public static class PasswordHasher
    {
      public static string HashPassword(string password)
      {
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            100000,
            HashAlgorithmName.SHA256,
            32);

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
      }

      // Verificar la contraseña ingresada contra el hash almacenado
      public static bool VerifyPassword(string password, string hashedPassword)
      {
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2)
          return false;

        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = Convert.FromBase64String(parts[1]);

        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            100000,
            HashAlgorithmName.SHA256,
            32);

        return CryptographicOperations.FixedTimeEquals(storedHash, hashToCompare);
      }
    }

    // Obtener usuario por Id
    public async Task<Usuario> ObtenerUsuarioPorId(int id)
    {
      return await _usuarioRepository.ObtenerPorId(id);
    }

    public async Task<List<Usuario>> ObtenerListadoUsuarios()
    {
      return await _usuarioRepository.ObtenerTodos();
    }

    // Registrar un nuevo usuario
    public async Task<ResponseMessages> RegistrarUsuarioAsync(Usuario nuevoUsuario)
    {
      // Validar si ya existe un usuario con el mismo email
      var usuarios = await _usuarioRepository.ObtenerTodos();
      var usuarioExistente = usuarios.Any(u => u.Tercero.Email == nuevoUsuario.Tercero.Email);

      if (usuarioExistente)
      {
        return new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "Usuario ya existe en la Base de Datos."
        };
      }

      // ✅ Hashear la contraseña antes de guardarla
      nuevoUsuario.Contrasena = PasswordHasher.HashPassword(nuevoUsuario.Contrasena);

      // Datos adicionales
      nuevoUsuario.FechaCreacion = DateTime.Now;
      nuevoUsuario.IdEstado = nuevoUsuario.IdEstado; // Ejemplo: activo por defecto

      // Guardar usuario
      await _usuarioRepository.Crear(nuevoUsuario);

      return new ResponseMessages
      {
        Exitoso = true,
        Mensaje = "Usuario registrado exitosamente."
      };
    }


    // Actualizar un usuario existente
    public async Task<bool> ActualizarUsuario(Usuario usuarioActualizado)
    {
      var usuarioExistente = await _usuarioRepository.ObtenerPorId(usuarioActualizado.IdUsuario);

      if (usuarioExistente == null)
        return false;

      // Actualizar datos de Usuario
      usuarioExistente.IdEstado = usuarioActualizado.IdEstado;
      usuarioExistente.IdRol = usuarioActualizado.IdRol;
      usuarioExistente.CodUsuario = usuarioActualizado.CodUsuario;
      usuarioExistente.FechaModificacion = DateTime.Now;
      usuarioExistente.IdUsuarioModificacion = usuarioActualizado.IdUsuarioModificacion;

      // ✅ Verificar si la contraseña cambió
      if (!string.IsNullOrWhiteSpace(usuarioActualizado.Contrasena) &&
          !PasswordHasher.VerifyPassword(usuarioActualizado.Contrasena, usuarioExistente.Contrasena))
      {
        // Hashear nueva contraseña
        usuarioExistente.Contrasena = PasswordHasher.HashPassword(usuarioActualizado.Contrasena);
      }

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

    public async Task<List<UsuarioResponse>> ObtenerUsuariosResponse()
    {
      try
      {

        var obtenerUsuarios = await _usuarioRepository.EjecutarProcedimientoAlmacenado<UsuarioResponse>("generales.cargar_data_usuarios_terceros");

        // Si se obtuvieron resultados, los devolvemos
        return obtenerUsuarios.ToList();
      }
      catch (Exception ex)
      {
        // Manejo de excepciones: si ocurre algún error, lanzamos una nueva excepción
        throw new Exception("Error al obtener roles por usuario", ex);
      }
    }

    public async Task<UsuarioResponse> ObtenerUsuarioPorIdTercero(int idTercero)
    {
      try
      {

        var usuario = await _usuarioRepository.EjecutarProcedimientoAlmacenado<UsuarioResponse>("generales.cargar_data_usuarios_terceros_por_id", idTercero);

        // Si se obtuvieron resultados, los devolvemos
        return usuario.FirstOrDefault();
      }
      catch (Exception ex)
      {
        // Manejo de excepciones: si ocurre algún error, lanzamos una nueva excepción
        throw new Exception("Error al obtener roles por usuario", ex);
      }
    }
  }
}


