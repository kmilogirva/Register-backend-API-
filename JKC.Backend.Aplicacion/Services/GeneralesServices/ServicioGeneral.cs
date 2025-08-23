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
    private readonly IRepository<TiposTercero> _repositoryTiposTercero;
    private readonly IRepository<TiposPersona> _repositoryTiposPersona;
    private readonly IRepository<Pais> _repositoryPais;
    private readonly IRepository<Departamento> _repositoryDepartamento;
    private readonly IRepository<Ciudad> _repositoryCiudad;
    private readonly IRepository<Tercero> _repositoryTercero;

    public ServicioGeneral(IRepository<TiposDocumento> repositoryTiposDocumento, IRepository<TiposTercero> repositoryTiposTercero, IRepository<TiposPersona> repositoryTiposPersona, IRepository<Pais> repositoryPais, IRepository<Departamento> repositoryDepartamento, IRepository<Ciudad> repositoryCiudad, IRepository<Tercero> repositoryTercero)
    {
      _repositoryTiposDocumento = repositoryTiposDocumento;
      _repositoryTiposTercero = repositoryTiposTercero;
      _repositoryTiposPersona = repositoryTiposPersona;
      _repositoryPais = repositoryPais;
      _repositoryDepartamento = repositoryDepartamento;
      _repositoryCiudad = repositoryCiudad;
      _repositoryTercero = repositoryTercero;
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

    public async Task<List<ComboResponse>> ObtenerComboTiposTercero()
    {

      var tiposTercero = await _repositoryTiposTercero.ObtenerTodos();

      var combo = tiposTercero.Select(t => new ComboResponse
      {
        Codigo = t.IdTipoTercero,
        Valor = t.NombreTipoTercero,
      }).ToList();

      return combo;

    }

    public async Task<List<ComboResponse>> ObtenerComboTiposPersona()
    {

      var tiposPersona = await _repositoryTiposPersona.ObtenerTodos();

      var combo = tiposPersona.Select(t => new ComboResponse
      {
        Codigo = t.IdTipoPersona,
        Valor = t.NombreTipoPersona,
      }).ToList();

      return combo;

    }
    public async Task<List<ComboResponse>> ObtenerComboPaises()
    {

      var comboPaises = await _repositoryPais.ObtenerTodos();

      var combo = comboPaises.Select(t => new ComboResponse
      {
        Codigo = t.IdPais,
        Valor = t.NombrePais,
      }).ToList();

      return combo;

    }

    public async Task<List<ComboResponse>> ObtenerComboDepartamentos(int idPais)
    {
      var comboDepartamentos = await _repositoryDepartamento.ObtenerAsync(
        filtro: d => d.IdPais == idPais
      );

      var combo = comboDepartamentos.Select(t => new ComboResponse
      {
        Codigo = t.IdDepartamento,
        Valor = t.NombreDepartamento,
      }).ToList();

      return combo;
    }

    public async Task<List<ComboResponse>> ObtenerComboCiudades(int idDepartamento)
    {
      var comboCiudades = await _repositoryCiudad.ObtenerAsync(
        filtro: d => d.IdDepartamento == idDepartamento
      );

      //var comboCiudades = await _repositoryCiudad.ObtenerTodosInclude(d => d.IdDepartamento == idDepartamento);

      var combo = comboCiudades.Select(t => new ComboResponse
      {
        Codigo = t.IdCiudad,
        Valor = t.NombreCiudad,
      }).ToList();

      return combo;
    }
  }
}
