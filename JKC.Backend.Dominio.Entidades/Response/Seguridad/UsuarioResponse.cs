using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Response.Seguridad
{
  public class UsuarioResponse
  {
    public int IdTercero { get; set; }
    public string NombreCompleto { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public int? IdUsuario { get; set; }
    public int? IdRol { get; set; }
    public string? NombreRol { get; set; }
    public string? Contrasena { get; set; }
    public string? CodUsuario { get; set; }
    public int IdEstado { get; set; }
  }
}
