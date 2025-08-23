using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Generales
{
  public class Tercero
  {
    [Key]
    public int IdTercero { get; set; }
    public string Nombre1 { get; set; }
    public string? Nombre2 { get; set; }
    public string Apellido1 { get; set; }
    public string? Apellido2 { get; set; }

    public string? NombreCompleto
    {
      get
      {
        return string.Join(" ", new[] {
            Nombre1,
            Nombre2,
            Apellido1,
            Apellido2
        }.Where(n => !string.IsNullOrWhiteSpace(n)));
      }
    }
    public int IdTipoIdentificacion { get; set; }
    public string codDocumento { get; set; }
    public int IdTipoTercero { get; set; }
    public int IdTipoPersona { get; set; }
    public int IdPais { get; set; }
    public int IdDepartamento { get; set; }
    public int IdCiudad { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public int IdEstado { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int IdUsuarioCreacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }

    [ForeignKey("IdTipoIdentificacion")]
    public TiposDocumento? TipoIdentificacion { get; set; }

    [ForeignKey("IdTipoPersona")]
    public TiposPersona? TipoPersona { get; set; }

    [ForeignKey("IdTipoTercero")]
    public TiposTercero? TipoTercero { get; set; }


  }
}
