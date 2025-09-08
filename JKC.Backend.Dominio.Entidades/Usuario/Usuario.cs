using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Usuario
{
  public class Usuario
  {
    [Key]
    public int IdUsuario { get; set; }
    public string Nombre1 { get; set; }
    public string? Nombre2 { get; set; }
    public string Apellido1 { get; set; }
    public string? Apellido2 { get; set; }
    public string NombreCompleto
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
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public int IdRol { get; set; }
    public int IdEstado { get; set; }
    public string Contrasena { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int IdUsuarioCreacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }

    // 🔹 Campo nuevo para almacenar el token de recuperación
    //    Se guarda cuando el usuario solicita restablecer contraseña.
    public string? TokenRecuperacion { get; set; }

    // 🔹 Campo nuevo para indicar la fecha/hora en que expira el token
    public DateTime? TokenExpiracion { get; set; }
  }
}
