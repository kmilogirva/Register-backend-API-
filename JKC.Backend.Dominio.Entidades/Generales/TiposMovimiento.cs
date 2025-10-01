using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Generales
{
  public class TiposMovimiento
  {
    [Key]
    public int IdTipoMovimiento { get; set; }
    public string NombreTipoMovimiento { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int IdUsuarioCreacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }
  }
}
