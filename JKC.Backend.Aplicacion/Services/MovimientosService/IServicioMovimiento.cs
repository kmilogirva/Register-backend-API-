
using System.Collections.Generic;
using System.Threading.Tasks;
using JKC.Backend.Dominio.Entidades.Movimientos;

namespace JKC.Backend.Aplicacion.Services.MovimientoServices
{
  public interface IServicioMovimiento
  {
    Task<Movimiento> RegistrarMovimiento(Movimiento movimiento);
    Task<List<Movimiento>> ObtenerListadoMovimientos();
    Task<Movimiento> ObtenerMovimientoPorId(int id);
    Task EliminarMovimientoPorId(int id);
    Task ActualizarMovimiento(Movimiento movimiento);
  }
}



