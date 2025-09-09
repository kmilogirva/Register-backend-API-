using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Generales
{
  public class Departamento
  {
    [Key]
    public int IdDepartamento { get; set; }
    public string NombreDepartamento { get; set; }
    public string CodigoDian { get; set; }
    public int IdPais { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int IdUsuarioCreacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }
  }
}
