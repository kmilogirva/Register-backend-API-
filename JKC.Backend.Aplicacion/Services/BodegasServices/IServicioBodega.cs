using System.Collections.Generic;
using System.Threading.Tasks;
using JKC.Backend.Dominio.Entidades;
using JKC.Backend.Dominio.Entidades.Bodegas;


namespace JKC.Backend.Aplicacion.Services.BodegaServices
{
  public interface IServicioBodega
  {
    Task<Bodega> RegistrarBodega(Bodega bodega);
    Task<List<Bodega>> ObtenerListadoBodegas();
    Task<Bodega> ObtenerBodegaPorId(int id);
    Task EliminarBodegaPorId(int? id);
  }
}
