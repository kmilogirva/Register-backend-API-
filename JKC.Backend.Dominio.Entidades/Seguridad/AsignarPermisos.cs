using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad
{
  public class AsignarPermisos
  {
    [Key]
    public int Id { get; set; }
    public int IdRol { get; set; }
    public int IdSubModulo { get; set; }
    public int Crear { get; set; }
    public int Editar { get; set; }
    public int Eliminar { get; set; }
    public int Leer { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int IdUsuarioCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }
  }
}
