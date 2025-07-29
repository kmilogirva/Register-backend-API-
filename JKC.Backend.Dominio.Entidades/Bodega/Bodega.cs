using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades
{
  public class Bodega
  {
    [Key]
    public int IdBodega { get; set; }
    public string NombreBodega { get; set; }
    public string Ubicacion { get; set; }
    public int CantidadMaxima { get; set; }
    public bool IdEstado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int IdUsuarioCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }
  }
}
