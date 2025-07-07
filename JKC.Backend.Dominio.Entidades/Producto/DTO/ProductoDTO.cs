using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Producto.DTO
{
  public class ProductoDto
  {
    public int IdProducto { get; set; }
    public string CodEan { get; set; }
    public string NomProducto { get; set; }
    public int IdCategoria { get; set; }
    public string UbicacionProducto { get; set; }
    public int Cantidad { get; set; }
    public string Observacion { get; set; }
  }
}
