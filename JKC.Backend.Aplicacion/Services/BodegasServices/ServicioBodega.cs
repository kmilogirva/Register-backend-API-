using System.Collections.Generic;
using System.Threading.Tasks;
using JKC.Backend.Aplicacion.Services.BodegaServices;
using JKC.Backend.Dominio.Entidades;
using JKC.Backend.Dominio.Entidades.Bodegas;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;

namespace JKC.Backend.Aplicacion.Services.BodegaServices
{
  public class ServicioBodega : IServicioBodega
  {
    private readonly IRepository<Bodega> _bodegaRepository;

    public ServicioBodega(IRepository<Bodega> bodegaRepository)
    {
      _bodegaRepository = bodegaRepository;
    }

    public async Task<Bodega> RegistrarBodega(Bodega bodega)
    {
      await _bodegaRepository.Crear(bodega);
      return bodega;
    }

    public async Task<List<Bodega>> ObtenerListadoBodegas()
    {
      return await _bodegaRepository.ObtenerTodos();
    }

    public async Task<Bodega> ObtenerBodegaPorId(int id)
    {
      return await _bodegaRepository.ObtenerPorId(id);
    }

    public async Task EliminarBodegaPorId(int? id)
    {
      await _bodegaRepository.EliminarPorId(id);
    }
  }
}
