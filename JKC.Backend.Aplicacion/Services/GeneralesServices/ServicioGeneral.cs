using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.GeneralesServices
{
  public class ServicioGeneral : IServicioGeneral
  {

    private readonly IRepository<TiposDocumento> _repositoryTiposDocumento;

    public ServicioGeneral(IRepository<TiposDocumento> repositoryTiposDocumento)
    {
      _repositoryTiposDocumento = repositoryTiposDocumento;
    }

    public async Task<List<ComboResponse>> ObtenerComboTiposDocumento()
    {

      var tiposDocumento = await _repositoryTiposDocumento.ObtenerTodos();

      var combo = tiposDocumento.Select(t => new ComboResponse
      {
        Codigo = t.IdTipoDocumento,
        Valor = t.NombreDocumento,
      }).ToList();

      return combo;

    }
  }
}
