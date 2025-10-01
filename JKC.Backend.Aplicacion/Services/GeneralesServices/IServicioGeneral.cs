using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.GeneralesServices
{

  public interface IServicioGeneral
  {
    Task <List<ComboResponse>> ObtenerComboTiposDocumento();
    Task<List<ComboResponse>> ObtenerComboTiposTercero();
    Task<List<ComboResponse>> ObtenerComboTiposPersona();
    Task<List<ComboResponse>> ObtenerComboPaises();
    Task<List<ComboResponse>> ObtenerComboDepartamentos(int idPais);
    Task<List<ComboResponse>> ObtenerComboCiudades(int idPais);
    Task<List<ComboResponse>> ObtenerComboTiposMovimiento();
  }
}
