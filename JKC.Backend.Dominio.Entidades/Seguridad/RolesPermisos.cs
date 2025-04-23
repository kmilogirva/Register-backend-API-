using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad.Usuarios
{
  public class RolesPermisos
  {
    [Key]
    public int Id { get; set; }
    public int IdRol { get; set; }
    public int IdPermiso { get; set; }
    public short SwPermisoActivo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaModifica { get; set; }
    public int IdUsuarioCrea { get; set; }
    public int IdUsuarioModifica { get; set; }

  }


}
