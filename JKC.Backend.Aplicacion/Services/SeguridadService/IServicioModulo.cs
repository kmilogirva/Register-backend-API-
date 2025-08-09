using JKC.Backend.Dominio.Entidades.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.SeguridadService
{
  public interface IServicioModulo
  {
    Task<Modulo> CrearModuloAsync(Modulo modulo);
    Task<Modulo?> ObtenerPorIdAsync(int id);
    Task<List<Modulo>> ObtenerTodosAsync();
    Task<Modulo> ActualizarModuloAsync(Modulo modulo);
    Task<bool> EliminarModuloAsync(int id);
  }
}
