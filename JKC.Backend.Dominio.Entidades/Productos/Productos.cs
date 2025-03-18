using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Productos
{
  public class Productos
  {
    [Key]
    public int IdProducto { get; set; }
    public string CodCategoria { get; set; }
    public string NomProducto { get; set;}
    public int IdCategoria { get; set; }
    public string UbicacionProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set;}
  }
}
