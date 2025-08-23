using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.GeneralesServices
{
  public interface IServicioTercero
  {
    Task<Tercero> ObtenerTerceroPorId(int id);
    Task<List<Tercero>> ObtenerListadoTerceros();
    Task<ResponseMessages> RegistrarTercero(Tercero nuevoTercero);
    Task<bool> ActualizarTercero(Tercero terceroActualizado);
    Task<bool> EliminarTerceroPorId(int id);
  }
}
