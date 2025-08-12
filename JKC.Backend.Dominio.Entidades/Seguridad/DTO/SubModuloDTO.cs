using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad.producto
{
  public class Submoduloproducto
  {
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = null!;
    public int IdModulo { get; set; }
    public string NombreModulo { get; set; } = null!;
    public int IdEstado { get; set; }
    public string Descripcion { get; set; }
    public string RutaAngular { get; set; }
  }

}
