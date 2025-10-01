using System.ComponentModel.DataAnnotations;

using System;
using System.ComponentModel.DataAnnotations;

namespace JKC.Backend.Dominio.Entidades.Movimientos
{
  //public enum TipoMovimientoEnum


  //{
  //  Entrada = 1,
  //  Salida = 2
  //}

  public class Movimiento
  {
    [Key]
    public int IdMovimiento { get; set; }
    public int IdTipoMovimiento { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public string? Observacion { get; set; }
    public int IdUsuarioCreacion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
  }
}
