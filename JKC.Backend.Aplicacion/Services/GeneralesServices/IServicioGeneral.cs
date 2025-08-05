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
  }
}
