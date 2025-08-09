using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{
  public interface IServicioSubModulo 
  {
    Task<SubModulo> CrearSubmodulo(SubModulo submodulo);

    Task<SubModulo?> ObtenerPorId(int id);

    Task<List<SubModulo>> ObtenerTodos();

    Task<List<SubmoduloDto>> ObtenerTodosInclude();

    Task<SubModulo> ActualizarSubmodulo(SubModulo submodulo);

    Task<bool> EliminarSubmodulo(int id);
    //Task List<Submodulo>> ObtenerTodos(int id);
  }
}
