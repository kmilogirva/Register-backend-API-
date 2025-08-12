using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad
{
  public class Modulo
  {
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? IconModulo { get; set; }

    public int IdEstado { get; set; }

    public DateTime? FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaModificacion { get; set; }

    public int IdUsuarioCreacion { get; set; }

    public int? IdUsuarioModificacion { get; set; }

    public ICollection<SubModulo>? Submodulos { get; set; }
  }

}
