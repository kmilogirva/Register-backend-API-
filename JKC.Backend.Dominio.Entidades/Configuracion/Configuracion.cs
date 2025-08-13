using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Generales
{
  public class Configuracion
  {
    [Key]
    public int Id { get; set; }
    // Identification
    [Required]
    [MaxLength(100)]
    public string Key { get; set; } // e.g., "MAX_LOGIN_ATTEMPTS"

    [MaxLength(255)]
    public string Description { get; set; } // e.g., "Maximum number of allowed login attempts"

    [MaxLength(50)]
    public string Category { get; set; } // e.g., "Security", "Notifications", "Billing"
    public string Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsActive { get; set; } 
    public int? IdUsuarioCreacion { get; set; }


  }
}
