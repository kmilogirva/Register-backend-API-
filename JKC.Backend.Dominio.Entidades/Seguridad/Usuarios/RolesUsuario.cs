using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad.Usuarios
{
  public class RolesUsuario
  {

    [Key]
    public int Id { get; set; }
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public bool SwRolActivo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int IdUsuarioCrea { get; set; }
    public int? IdUsuarioModifica { get; set; }
  }
}
