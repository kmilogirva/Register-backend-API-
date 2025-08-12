using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad.Usuarios.producto
{
  public class PermisoModuloproducto
  {
    public int IdModulo { get; set; }
    public string NomModulo { get; set; }
    public int? IdModuloPadre { get; set; }
    public string NomModuloPadre { get; set; }
    public int IdPermiso { get; set; }
    public string NomPermiso { get; set; }
  }

}
