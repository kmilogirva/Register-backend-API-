using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.producto;
using JKC.Backend.Dominio.Entidades.Usuario;
using BCrypt.Net; // <-- para hashear contraseñas

namespace JKC.Backend.Aplicacion.Services.UsuarioServices
{
  public class ServicioUsuario : IServicioUsuario
  {

    private readonly IRepository<Usuario> _usuarioRepository;

    public ServicioUsuario(IRepository<Usuario> usuarioRepository)
    {
      _usuarioRepository = usuarioRepository;
    }

    // Obtener usuario por Id
    public async Task<Usuario> ObtenerUsuarioPorId(int id)
    {
      return await _usuarioRepository.ObtenerPorId(id);
    }

    // NUEVO: Obtener usuario por correo
    public async Task<Usuario> ObtenerUsuarioPorCorreo(string correo)
    {
      var usuarios = await _usuarioRepository.ObtenerTodos();
      return usuarios.FirstOrDefault(u => u.Correo == correo);
    }

    public async Task<List<Usuario>> ObtenerListadoUsuarios()
    {
      return await _usuarioRepository.ObtenerTodos();
    }

    // Registrar un nuevo usuario (ya tenía hashing en versiones previas; asegúrate de mantenerlo)
    public async Task<ResponseMessages> RegistrarUsuarioAsync(Usuario nuevoUsuario)
    {
      var usuarios = await _usuarioRepository.ObtenerTodos();
      var usuarioExistente = usuarios.Any(u => u.Correo == nuevoUsuario.Correo);

      if (usuarioExistente)
      {
        return new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "Usuario ya existe en la Base de Datos."
        };
      }

      // Hashear antes de guardar (si no está hasheada)
      if (!string.IsNullOrWhiteSpace(nuevoUsuario.Contrasena))
      {
        nuevoUsuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Contrasena, workFactor: 12);
      }

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

      usuarioExistente.Nombre1 = usuarioActualizado.Nombre1;
      usuarioExistente.Nombre2 = usuarioActualizado.Nombre2;
      usuarioExistente.Apellido1 = usuarioActualizado.Apellido1;
      usuarioExistente.Apellido2 = usuarioActualizado.Apellido2;
      usuarioExistente.Correo = usuarioActualizado.Correo;
      usuarioExistente.Telefono = usuarioActualizado.Telefono;
      usuarioExistente.IdEstado = usuarioActualizado.IdEstado;

      // Si se actualiza contraseña, la hasheamos
      if (!string.IsNullOrWhiteSpace(usuarioActualizado.Contrasena))
      {
        usuarioExistente.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuarioActualizado.Contrasena, workFactor: 12);
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

    // ====================================================
    // NUEVO: Solicitar recuperación de contraseña (genera token y guarda en BD)
    // ====================================================
    public async Task<ResponseMessages> SolicitarRecuperacionContrasenaAsync(string correo)
    {
      var usuarios = await _usuarioRepository.ObtenerTodos();
      var usuario = usuarios.FirstOrDefault(u => u.Correo == correo);

      if (usuario == null)
      {
        return new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "No existe una cuenta asociada a ese correo."
        };
      }

      // Generar token único y expiración
      string token = Guid.NewGuid().ToString("N"); // sin guiones
      usuario.TokenRecuperacion = token;
      usuario.TokenExpiracion = DateTime.UtcNow.AddHours(1);

      await _usuarioRepository.Actualizar(usuario);

      return new ResponseMessages
      {
        Exitoso = true,
        Mensaje = "Token generado correctamente."
        // NOTA: por seguridad no devolvemos token aquí. El controller lo obtendrá desde DB si es necesario.
      };
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
      usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena, workFactor: 12);
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
