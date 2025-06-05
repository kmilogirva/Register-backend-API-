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
    public string CodCategoria { get; set; }
    public string NomProducto { get; set;}

    [ForeignKey("Categoria")]
    public int IdCategoria { get; set; }
    public Categoria Categoria { get; set; }
    public string UbicacionProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set;}
  }
}
