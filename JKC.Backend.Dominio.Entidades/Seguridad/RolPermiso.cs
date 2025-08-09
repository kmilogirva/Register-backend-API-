using JKC.Backend.Dominio.Entidades.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad
{
  public class RolPermiso
  {
    [Key]
    public int IdRolPermiso { get; set; }
    public int IdRol { get; set; }
    public int IdSubModulo { get; set; }
    public string? NombreSubModulo { get; set; }
    public int IdModulo { get; set; }
    public string? NombreModulo { get; set; }
    public bool Leer { get; set; } = false;
    public bool Crear { get; set; } = false;
    public bool Editar { get; set; } = false;
    public bool Eliminar { get; set; } = false;

    // Relaciones de navegación
    //[ForeignKey("IdRol")]
    //public virtual Rol Rol { get; set; } = null!;

    //[ForeignKey("IdSubModulo")]
    //public virtual SubModulo SubModulo { get; set; } = null!;
  }
}
  






