using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.producto;
using JKC.Backend.Dominio.Entidades.Usuario;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{
  public class ServicioSeguridad : IServicioSeguridad
  {
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IRepository<Roles> _rolesRepository;
    private readonly IRepository<AsignarPermisos> _asignarPermisos;

    public ServicioSeguridad(IRepository<Usuario> usuarioRepository, IRepository<Roles> rolesRepository, IRepository<AsignarPermisos> asignarPermisos)
    {
      _usuarioRepository = usuarioRepository;
      _rolesRepository = rolesRepository;
      _asignarPermisos = asignarPermisos;
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

    public async Task<Roles> ActualizarRol(int id, Roles rolActualizado)
    {
      var rolExistente = await _rolesRepository.ObtenerPorId(id);

      if (rolExistente == null)
        return null;

      rolExistente.NombreRol = rolActualizado.NombreRol;
      rolExistente.IdEstado = rolActualizado.IdEstado;
      rolExistente.FechaModificacion = DateTime.UtcNow;
      rolExistente.IdUsuarioModificacion = rolActualizado.IdUsuarioModificacion;

      await _rolesRepository.Actualizar(rolExistente);

      return rolExistente;
    }

    public async Task<ResponseMessagesData<UsuarioDto>> LoginAsync(string? email, string? codUsuario, string password)
    {
      if ((string.IsNullOrEmpty(email) && string.IsNullOrEmpty(codUsuario)) || string.IsNullOrEmpty(password))
      {
        return new ResponseMessagesData<UsuarioDto>
        {
          Exitoso = false,
          Mensaje = "El correo/código de usuario o la contraseña no pueden estar vacíos.",
          Data = null
        };
      }

      var usuarios = await _usuarioRepository.ObtenerTodosInclude(u => u.Tercero);

      var usuarioAutorizado = usuarios.FirstOrDefault(u =>
          u.IdEstado == 1 &&
          (
              (!string.IsNullOrEmpty(email) && u.Tercero.Email == email) ||
              (!string.IsNullOrEmpty(codUsuario) && u.CodUsuario == codUsuario)
          )
      );

      if (usuarioAutorizado == null)
      {
        return new ResponseMessagesData<UsuarioDto>
        {
          Exitoso = false,
          Mensaje = "Credenciales inválidas",
          Data = null
        };
      }

      // ==================================================================
      // CAMBIO 1: Simplificación del Login para la Exposición
      // ==================================================================
      // Se comenta la verificación por Hash y se reemplaza por una comparación de texto simple.
      // bool passwordValida = PasswordHasher.VerifyPassword(password, usuarioAutorizado.Contrasena);

      // ADVERTENCIA: Solo para fines académicos. No usar en un proyecto real.
      bool passwordValida = (password == usuarioAutorizado.Contrasena);
      // ==================================================================

      if (!passwordValida)
      {
        return new ResponseMessagesData<UsuarioDto>
        {
          Exitoso = false,
          Mensaje = "Credenciales inválidas",
          Data = null
        };
      }

      var usuarioproducto = new UsuarioDto
      {
        IdUsuario = usuarioAutorizado.IdUsuario,
        CodUsuario = usuarioAutorizado.CodUsuario,
        Nombre = usuarioAutorizado.Tercero.NombreCompleto,
        Correo = usuarioAutorizado.Tercero.Email,
        IdRol = usuarioAutorizado.IdRol
      };

      return new ResponseMessagesData<UsuarioDto>
      {
        Exitoso = true,
        Mensaje = "Inicio de sesión exitoso",
        Data = usuarioproducto
      };
    }

    public async Task<List<Roles>> ObtenerListadoRoles()
    {
      return await _rolesRepository.ObtenerTodos();
    }

    public async Task<List<RolPermisosAccionproducto>> ObtenerPermisosPorRol(int idRol)
    {
      try
      {
        // ==================================================================
        // CAMBIO 2: Llamada al Procedimiento Almacenado Corregido
        // ==================================================================
        var resultado = await _rolesRepository.EjecutarProcedimientoAlmacenado<RolPermisosAccionproducto>(
            "seguridad.ObtenerPermisosPorRol_Corregido", idRol
        );
        // ==================================================================
        return resultado.ToList();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public async Task<List<RolPermisosAccionproducto>> CrearPermisosRolesAcciones(List<AsignarPermisos> asignarPermisosLista)
    {
      if (asignarPermisosLista == null || !asignarPermisosLista.Any())
        throw new ArgumentException("La lista de permisos está vacía.");

      var idRol = asignarPermisosLista.FirstOrDefault()?.IdRol ?? 0;
      if (idRol == 0)
        throw new ArgumentException("El IdRol no es válido en la lista de permisos.");

      var permisosexistentes = (await _asignarPermisos.ObtenerTodos())
                                .Where(p => p.IdRol == idRol)
                                .ToList();

      foreach (var permiso in permisosexistentes)
      {
        await EliminarPermisosPorId(permiso.Id);
      }

      foreach (var permiso in asignarPermisosLista)
      {
        permiso.FechaCreacion = DateTime.UtcNow;
        await _asignarPermisos.Crear(permiso);
      }

      return await ObtenerPermisosPorRol(idRol);
    }

    public async Task<bool> EliminarPermisosPorId(int Id)
    {
      try
      {
        await _asignarPermisos.EliminarPorId(Id);
        return true;
      }
      catch (Exception ex)
      {
        throw new Exception("Error al eliminar los permisos del rol", ex);
      }
    }

    public async Task<bool> EliminarRol(int id)
    {
      var rol = await _rolesRepository.ObtenerPorId(id);
      if (rol == null)
        return false;

      await _rolesRepository.Eliminar(rol);
      return true;
    }

    public async Task<string> ObtenerMenuJsonDesdeBaseDeDatos(int idRol)
    {
      try
      {
        var resultado = await _rolesRepository.EjecutarProcedimientoAlmacenado<string>(
           "seguridad.ObtenerJsonPermisosAsignadosPorRol", idRol
       );
        return resultado.FirstOrDefault();
      }
      catch (Exception ex)
      {
        throw new Exception("Error al obtener el menú desde la base de datos", ex);
      }
    }
  }
}
