using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.DTOS.Usuarios
{
  public class UsuarioDto
  {
    public int IdUsuario { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
  }
}
