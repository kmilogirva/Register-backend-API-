using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad.Usuarios
{
  public class Usuario
  {
    [Key]
    public int IdUsuario { get; set; }

    public string Nombres { get; set; }

    public string Apellidos { get; set; }

    public string Correo { get; set; }

    public string Telefono { get; set; }

    public string Sexo { get; set; }

    public string Contrasena { get; set; }

    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public int IdUsuarioCreacion { get; set; }
    public int? IdUsuarioModificacion { get; set; }


  }
}
