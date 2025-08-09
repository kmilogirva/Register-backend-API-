using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad.DTO
{
  public class RolPermisosAccionDTO
  {
    public int? Id { get; set; }
    public int? IdRol { get; set; }
    public int IdModulo { get; set; }
    public string NombreModulo { get; set; } = null!;
    public int IdSubModulo { get; set; }
    public string NombreSubModulo { get; set; }
    //public string Nombre { get; set; } = null!;
    public int IdEstado { get; set; }
    public int Leer { get; set; }
    public int Crear { get; set; }
    public int Editar { get; set; }
    public int Eliminar { get; set; }
  }
}
