using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Dominio.Entidades.Seguridad
{
    public class SubModulo
    {
      [Key]
      public int IdSubModulo { get; set; }

      public string Nombre { get; set; } = null!;

      public string? Descripcion { get; set; } = null!;

      public int IdModulo { get; set; }

      public int IdEstado { get; set; }

      public DateTime FechaCreacion { get; set; }

      public int IdUsuarioCreacion { get; set; }

      public DateTime? FechaModificacion { get; set; }

      public int? IdUsuarioModificacion { get; set; }

      // Relaciones de navegación (opcional, si usas EF Core)
      public Modulo? Modulo { get; set; }

      //public Estado? Estado { get; set; }
    }
  }

