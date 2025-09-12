using JKC.Backend.Dominio.Entidades.Generales;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Usuario
{
  public class Usuario
  {
    [Key]
    public int IdUsuario { get; set; }
    public int IdTercero { get; set; }
    public string CodUsuario { get; set; }
    public int IdRol { get; set; }
    public int IdEstado { get; set; }
    public string? Contrasena { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int? IdUsuarioCreacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }


    // 🔹 Campo nuevo para almacenar el token de recuperación
    //    Se guarda cuando el usuario solicita restablecer contraseña.
    public string? TokenRecuperacion { get; set; }

    // 🔹 Campo nuevo para indicar la fecha/hora en que expira el token
    public DateTime? TokenExpiracion { get; set; }
    [ForeignKey("IdTercero")]
    public Tercero? Tercero { get; set; }
  }
}
