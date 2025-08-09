using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Aplicacion.Services.DTOS.Usuarios;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.DTO;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.DTO;
using JKC.Backend.Dominio.Entidades.Usuario;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{
  public class ServicioSeguridad : IServicioSeguridad
  {
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IRepository<Rol> _rolesRepository;
    private readonly IRepository<AsignarPermisos> _asignarPermisos;

    public ServicioSeguridad(IRepository<Usuario> usuarioRepository, IRepository<Rol> rolesRepository, IRepository<AsignarPermisos> asignarPermisos)
    {
      _usuarioRepository = usuarioRepository;
      _rolesRepository = rolesRepository;
      _asignarPermisos = asignarPermisos;
    }

    // Firma simple: Task<Roles>
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

    public async Task<List<Rol>> ObtenerListadoRoles()
    {
      return await _rolesRepository.ObtenerTodos();
    }

    public async Task<List<RolPermisosAccionDTO>> ObtenerPermisosPorRol(int idRol)
    {
      try
      {
        var resultado = await _rolesRepository.EjecutarProcedimientoAlmacenado<RolPermisosAccionDTO>(
            "seguridad.ObtenerPermisosPorRol",  idRol
        );
        return resultado.ToList();
      }
      catch (Exception ex)
      {
        throw; // o lanza un mensaje más útil si lo necesitas
      }
    }

    public async Task<List<RolPermisosAccionDTO>> CrearPermisosRolesAcciones(List<AsignarPermisos> asignarPermisosLista)
    {
      if (asignarPermisosLista == null || !asignarPermisosLista.Any())
        throw new ArgumentException("La lista de permisos está vacía.");

      //var permisosexistentes = _rolesRepository.ObtenerTodos().Result
      //  .Where(p => p.Id == asignarPermisosLista.FirstOrDefault().IdRol)
      //  .ToList();

      var idRol = asignarPermisosLista.FirstOrDefault()?.IdRol ?? 0;

      var permisosexistentes = (await _asignarPermisos.ObtenerTodos())
          .Where(p => p.IdRol == idRol)
          .ToList();

      foreach (var permiso in permisosexistentes)
      {
        EliminarPermisosPorRol(permiso.Id);
      }

      // 2️⃣ Insertar los nuevos permisos
     
      foreach (var permiso in asignarPermisosLista)
      {
        permiso.FechaCreacion = DateTime.UtcNow;
        await _asignarPermisos.Crear(permiso);
      }

      // 3️⃣ Retornar lista actualizada
      return await ObtenerPermisosPorRol(idRol);
    }

        public async Task<bool> EliminarPermisosPorId(int Id)
        {
            try
            {
                //var permisosExistentes = await ObtenerPermisosPorRol(Id);

                //foreach (var permiso in permisosExistentes)
                //{
                await _asignarPermisos.EliminarPorId(Id);
                //}

                return true; // Si llega aquí, todo salió bien
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar los permisos del rol", ex);
            }

  }
}
