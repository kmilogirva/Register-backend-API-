using JKC.Backend.Aplicacion.Services.GeneralesServices;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Dominio.Entidades.Generales;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.GeneralesController
{
  public class GeneralesController : Controller
  {
    private readonly IServicioGeneral _generalesServicio;


    public GeneralesController(IServicioGeneral generalesServicio)
    {
      _generalesServicio = generalesServicio;
    }


    [HttpGet("combo-tipos-documento")]
    public async Task<IActionResult> ObtenerComboRoles()
    {
      try
      {
        var tiposDocumento = await _generalesServicio.ObtenerComboTiposDocumento();
        return Ok(tiposDocumento);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al obtener los roles.", error = ex.Message });
      }
    }
  }
}
