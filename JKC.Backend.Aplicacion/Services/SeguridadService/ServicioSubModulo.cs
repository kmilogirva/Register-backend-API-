using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.DTO;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{
  public class ServicioSubModulo : IServicioSubModulo
  {
    private readonly IRepository<SubModulo> _repositorio;

    public ServicioSubModulo(IRepository<SubModulo> repositorio)
    {
      _repositorio = repositorio;
    }

    public async Task<SubModulo> CrearSubmodulo(SubModulo submodulo)
    {
      submodulo.FechaCreacion = DateTime.UtcNow;
      await _repositorio.Crear(submodulo);
      return submodulo;
    }

    public async Task<SubModulo?> ObtenerPorId(int id)
    {
      return await _repositorio.ObtenerPorId(id);
    }

    public async Task<List<SubModulo>> ObtenerTodos()
    {
      return await _repositorio.ObtenerTodos();
    }

    public async Task<List<SubmoduloDto>> ObtenerTodosInclude()
    {
      var submodulos = await _repositorio.ObtenerTodosInclude(s => s.Modulo);

      var resultado = submodulos.Select(s => new SubmoduloDto
      {
        IdSubModulo = s.IdSubModulo,
        IdModulo = s.IdModulo,
        Nombre = s.Nombre,
        NombreModulo = s.Modulo.Nombre,
        IdEstado = s.IdEstado
      }).ToList();

      return resultado;
    }

    public async Task<SubModulo> ActualizarSubmodulo(SubModulo submodulo)
    {

      var submoduloexistente = await _repositorio.ObtenerPorId(submodulo.IdSubModulo);

      if (submoduloexistente != null)
      {
        submoduloexistente.Nombre = submodulo.Nombre;
        submoduloexistente.Descripcion = submodulo.Descripcion;
        submoduloexistente.IdModulo = submodulo.IdModulo;
        submoduloexistente.IdEstado = submodulo.IdEstado;
        submoduloexistente.FechaModificacion = DateTime.UtcNow;
        submoduloexistente.IdUsuarioModificacion = submodulo.IdUsuarioModificacion;
      }

      submodulo.FechaModificacion = DateTime.UtcNow;
      await _repositorio.Actualizar(submoduloexistente);
      return submodulo;
    }

    public async Task<bool> EliminarSubmodulo(int id)
    {
      await _repositorio.EliminarPorId(id);
      return true;
    }
  }
}
