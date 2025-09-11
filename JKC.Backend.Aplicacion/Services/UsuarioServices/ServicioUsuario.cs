using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Response.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.producto;
using JKC.Backend.Dominio.Entidades.Usuario;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace JKC.Backend.Aplicacion.Services.UsuarioServices
{
  public class ServicioUsuario : IServicioUsuario
  {

    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IRepository<Tercero> _terceroRepository;
    private readonly ILogger<ServicioUsuario> _logger;

    public ServicioUsuario(IRepository<Usuario> usuarioRepository, ILogger<ServicioUsuario> logger, IRepository<Tercero> terceroRepository)
    {
      _usuarioRepository = usuarioRepository;
      _terceroRepository = terceroRepository;
      _logger = logger;
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

    public async Task<List<UsuarioResponse>> ObtenerUsuariosResponse()
    {
      try
      {
        var usuarios = await _usuarioRepository.EjecutarProcedimientoAlmacenado<UsuarioResponse>("generales.cargar_data_usuarios_terceros");

        return usuarios.ToList();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error al cargar usuarios desde SP.");
        return new List<UsuarioResponse>();
      }
    }

    // NUEVO: Obtener usuario por correo
    //public async Task<Usuario> ObtenerUsuarioPorCorreo(string correo)
    //{
    //  var usuarios = await _usuarioRepository.ObtenerTodos();
    //  return usuarios.FirstOrDefault(u => u.emai == correo);
    //}

    public async Task<List<Usuario>> ObtenerListadoUsuarios()
    {
      return await _usuarioRepository.ObtenerTodos();
    }

    // Registrar un nuevo usuario (ya tenía hashing en versiones previas; asegúrate de mantenerlo)
    public async Task<ResponseMessages> RegistrarUsuarioAsync(Usuario nuevoUsuario)
    {
      try
      {
        var usuarioExistente = await _usuarioRepository
          .AnyAsync(u => u.IdTercero == nuevoUsuario.IdTercero);

        if (usuarioExistente)
        {
          _logger.LogWarning("Intento de registro fallido. El usuario con email {Email} ya existe.", nuevoUsuario.Tercero.Email);

          return new ResponseMessages
          {
            Exitoso = false,
            Mensaje = "Usuario ya existe en la Base de Datos."
          };
        }

        // Hash de contraseña
        nuevoUsuario.Contrasena = PasswordHasher.HashPassword(nuevoUsuario.Contrasena);
        nuevoUsuario.FechaCreacion = DateTime.Now;

        await _usuarioRepository.Crear(nuevoUsuario);


        return new ResponseMessages
        {
          Exitoso = true,
          Mensaje = "Usuario registrado exitosamente."
        };
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error al registrar usuario con email {Email}", nuevoUsuario.Tercero.Email);
        return new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "Ocurrió un error al registrar el usuario."
        };
      }
    }


    // Actualizar un usuario existente
    public async Task<bool> ActualizarUsuario(Usuario usuarioActualizado)
    {
      try
      {
        var usuarioExistente = await _usuarioRepository.ObtenerPorId(usuarioActualizado.IdUsuario);

        if (usuarioExistente == null)
        {
          _logger.LogWarning("No se encontró el usuario con ID {Id} para actualizar.", usuarioActualizado.IdUsuario);
          return false;
        }

        usuarioExistente.IdEstado = usuarioActualizado.IdEstado;
        usuarioExistente.IdRol = usuarioActualizado.IdRol;
        usuarioExistente.CodUsuario = usuarioActualizado.CodUsuario;
        usuarioExistente.FechaModificacion = DateTime.Now;
        usuarioExistente.IdUsuarioModificacion = usuarioActualizado.IdUsuarioModificacion;

        // Verificar si la contraseña cambió
        if (!string.IsNullOrWhiteSpace(usuarioActualizado.Contrasena) &&
            !PasswordHasher.VerifyPassword(usuarioActualizado.Contrasena, usuarioExistente.Contrasena))
        {
          usuarioExistente.Contrasena = PasswordHasher.HashPassword(usuarioActualizado.Contrasena);
        }

        await _usuarioRepository.Actualizar(usuarioExistente);

        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error al actualizar usuario con ID {Id}", usuarioActualizado.IdUsuario);
        return false;
      }
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

    public async Task<List<RolesUsuario>> ObtenerRolesPorIdUsuario(int idUsuario)
    {
      try
      {
        var obtenerRolesUsuarios = await _usuarioRepository.EjecutarProcedimientoAlmacenado<RolesUsuario>("obtenerRolesUsuario", idUsuario);
        return obtenerRolesUsuarios.ToList();
      }
      catch (Exception ex)
      {
        throw new Exception("Error al obtener roles por usuario", ex);
      }
    }

    public async Task<UsuarioResponse> ObtenerUsuarioPorIdTercero(int idTercero)
    {
      try
      {
        var usuario = await _usuarioRepository.EjecutarProcedimientoAlmacenado<UsuarioResponse>(
            "generales.cargar_data_usuarios_terceros_por_id", idTercero);

        if (!usuario.Any())
        {
          _logger.LogWarning("No se encontró usuario asociado al tercero con ID {IdTercero}", idTercero);
          return null;
        }

        return usuario.FirstOrDefault();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error al obtener usuario por IdTercero {IdTercero}", idTercero);
        return null;
      }
    }

    //// Fix for CS1001: Add the missing parameter name in the method signature
    public async Task<(ResponseMessages Respuesta, string Token)> SolicitarRecuperacionContrasenaAsync(string correo)
    {
      var usuarios = await _usuarioRepository.ObtenerTodosInclude(u => u.Tercero);

      var usuario = usuarios.FirstOrDefault(u => u.Tercero.Email == correo);

      if (usuario == null)
      {
        return (new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "No existe una cuenta asociada a ese correo."
        }, null);
      }

      // Generar token único y expiración
      string token = Guid.NewGuid().ToString("N");
      usuario.TokenRecuperacion = token;
      usuario.TokenExpiracion = DateTime.UtcNow.AddHours(1);

      await _usuarioRepository.Actualizar(usuario);

      return (new ResponseMessages
      {
        Exitoso = true,
        Mensaje = "Token generado correctamente."
      }, token);
    }


    // ====================================================
    // NUEVO: Restablecer contraseña usando token
    // ====================================================
    public async Task<ResponseMessages> RestablecerContrasenaAsync(string token, string nuevaContrasena)
    {
      if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(nuevaContrasena))
      {
        return new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "Token o nueva contraseña inválida."
        };
      }

      var usuarios = await _usuarioRepository.ObtenerTodos();
      var usuario = usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);

      if (usuario == null)
      {
        return new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "Token inválido."
        };
      }

      // Verificar expiración
      if (usuario.TokenExpiracion == null || usuario.TokenExpiracion < DateTime.UtcNow)
      {
        return new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "Token vencido. Solicite nuevamente la recuperación."
        };
      }

      // Hashear y guardar nueva contraseña. Limpiar token.
      usuario.Contrasena = PasswordHasher.HashPassword(nuevaContrasena);
      usuario.TokenRecuperacion = null;
      usuario.TokenExpiracion = null;

      await _usuarioRepository.Actualizar(usuario);

      return new ResponseMessages
      {
        Exitoso = true,
        Mensaje = "Contraseña actualizada correctamente."
      };
    }
  }
}

    //public async Task<List<UsuarioResponse>> ObtenerUsuariosResponse()
    //{
    //  try
    //  {
    //    var usuarios = await _usuarioRepository.EjecutarProcedimientoAlmacenado<UsuarioResponse>("generales.cargar_data_usuarios_terceros");

    //    return usuarios.ToList();
    //  }
    //  catch (Exception ex)
    //  {
    //    _logger.LogError(ex, "Error al cargar usuarios desde SP.");
    //    return new List<UsuarioResponse>();
    //  }
    //}


    //    public async Task<UsuarioResponse> ObtenerUsuarioPorIdTercero(int idTercero)
    //    {
    //      try
    //      {
    //        var usuario = await _usuarioRepository.EjecutarProcedimientoAlmacenado<UsuarioResponse>(
    //            "generales.cargar_data_usuarios_terceros_por_id", idTercero);

    //        if (!usuario.Any())
    //        {
    //          _logger.LogWarning("No se encontró usuario asociado al tercero con ID {IdTercero}", idTercero);
    //          return null;
    //        }

    //        return usuario.FirstOrDefault();
    //      }
    //      catch (Exception ex)
    //      {
    //        _logger.LogError(ex, "Error al obtener usuario por IdTercero {IdTercero}", idTercero);
    //        return null;
    //      }
    //    }
    //  }
    //}

  //  public async Task<ResponseMessages> RestablecerContrasenaAsync(string token, string nuevaContrasena)
  //  {
  //    if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(nuevaContrasena))
  //    {
  //      return new ResponseMessages
  //      {
  //        Exitoso = false,
  //        Mensaje = "Token o nueva contraseña inválida."
  //      };
  //    }

  //    var usuarios = await _usuarioRepository.ObtenerTodos();
  //    var usuario = usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);

  //    if (usuario == null)
  //    {
  //      return new ResponseMessages
  //      {
  //        Exitoso = false,
  //        Mensaje = "Token inválido."
  //      };
  //    }

  //    // Verificar expiración
  //    if (usuario.TokenExpiracion == null || usuario.TokenExpiracion < DateTime.UtcNow)
  //    {
  //      return new ResponseMessages
  //      {
  //        Exitoso = false,
  //        Mensaje = "Token vencido. Solicite nuevamente la recuperación."
  //      };
  //    }

  //    // Hashear y guardar nueva contraseña. Limpiar token.
  //    usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena, workFactor: 12);
  //    usuario.TokenRecuperacion = null;
  //    usuario.TokenExpiracion = null;

  //    await _usuarioRepository.Actualizar(usuario);

  //    return new ResponseMessages
  //    {
  //      Exitoso = true,
  //      Mensaje = "Contraseña actualizada correctamente."
  //    };
  //  }
  //}

