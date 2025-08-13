using JKC.Backend.Dominio.Entidades.Categorias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Productos
{
  public class Producto
  {
    [Key]
    public int IdProducto { get; set; }
    public string CodEan { get; set; }
    public string NomProducto { get; set; }
    //public Categoria IdCategoria { get; set; }
    public int IdCategoria { get; set; }
    public string UbicacionProducto { get; set; }
    public int Cantidad { get; set; }
    public string Observacion { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int? IdUsuarioCreacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }
  }
}
