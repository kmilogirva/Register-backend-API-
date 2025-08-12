using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{
  public class ServicioModulo : IServicioModulo
  {
    private readonly IRepository<Modulo> _repositorio;

    public ServicioModulo(IRepository<Modulo> repositorio)
    {
      _repositorio = repositorio;
    }

    public async Task<Modulo> CrearModuloAsync(Modulo modulo)
    {
      modulo.FechaCreacion = DateTime.UtcNow;
      await _repositorio.Crear(modulo);
      return modulo;
    }

    public async Task<Modulo?> ObtenerPorIdAsync(int id)
    {
      return await _repositorio.ObtenerPorId(id);
    }

    public async Task<List<Modulo>> ObtenerTodosAsync()
    {
      return await _repositorio.ObtenerTodos();
    }

    public async Task<Modulo> ActualizarModuloAsync(Modulo modulo)
    {

      var moduloExistente = await _repositorio.ObtenerPorId(modulo.Id);

      if (moduloExistente != null)
      {
        moduloExistente.Nombre = modulo.Nombre;
        moduloExistente.Descripcion = modulo.Descripcion;
        moduloExistente.Id = modulo.Id;
        moduloExistente.IdEstado = modulo.IdEstado;
        moduloExistente.FechaModificacion = DateTime.UtcNow;
        moduloExistente.IdUsuarioModificacion = modulo.IdUsuarioModificacion;
        moduloExistente.IconModulo = modulo.IconModulo;
      }

      await _repositorio.Actualizar(moduloExistente);
      return modulo;
    }

    public async Task<bool> EliminarModuloAsync(int id)
    {
      await _repositorio.EliminarPorId(id);
      return true;
    }
  }
}
