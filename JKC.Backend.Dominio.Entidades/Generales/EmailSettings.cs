using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Generales
{
  public class EmailSettings
  {
    public string SmtpHost { get; set; }   // Ej: "smtp.gmail.com"
    public int SmtpPort { get; set; }      // Ej: 587
    public string Email { get; set; }      // Tu cuenta Gmail
    public string Password { get; set; }   // Contraseña de app
  }
}
