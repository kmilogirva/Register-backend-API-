using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad.RecuperacionClave
{
  public class RestablecerContrasenaDto
  {
    public string Token { get; set; }
    public string NuevaContrasena { get; set; }
  }
}
