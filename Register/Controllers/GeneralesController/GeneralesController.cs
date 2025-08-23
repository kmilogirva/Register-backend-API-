using JKC.Backend.Aplicacion.Services.GeneralesServices;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Dominio.Entidades.Generales;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.GeneralesController
{

  [ApiController]
  [Route("api/[controller]")]
  public class GeneralesController : Controller
  {
    private readonly IServicioGeneral _generalesServicio;


    public GeneralesController(IServicioGeneral generalesServicio)
    {
      _generalesServicio = generalesServicio;
    }


    [HttpGet("combo-tipos-documento")]
    public async Task<IActionResult> ObtenerComboTiposDocumento()
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

    [HttpGet("combo-tipos-terceros")]
    public async Task<IActionResult> ObtenerComboTiposTercero()
    {
      try
      {
        var tiposTercero = await _generalesServicio.ObtenerComboTiposTercero();
        return Ok(tiposTercero);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al obtener los roles.", error = ex.Message });
      }
    }

    [HttpGet("combo-tipos-persona")]
    public async Task<IActionResult> ObtenerComboTiposPersona()
    {
      try
      {
        var tiposPersona = await _generalesServicio.ObtenerComboTiposPersona();
        return Ok(tiposPersona);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al obtener los roles.", error = ex.Message });
      }
    }

    [HttpGet("combo-paises")]
    public async Task<IActionResult> ObtenerComboPaises()
    {
      try
      {
        var paises = await _generalesServicio.ObtenerComboPaises();
        return Ok(paises);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al obtener los roles.", error = ex.Message });
      }
    }

    [HttpGet("combo-departamentos/{idPais}")]
    public async Task<IActionResult> ObtenerComboDepartamentos(int idPais)
    {
      try
      {
        var departamentos = await _generalesServicio.ObtenerComboDepartamentos(idPais);
        return Ok(departamentos);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al obtener los roles.", error = ex.Message });
      }
    }

    [HttpGet("combo-ciudades/{idDepartamento}")]
    public async Task<IActionResult> ObtenerComboCiudades(int idDepartamento)
    {
      try
      {
        var ciudades = await _generalesServicio.ObtenerComboCiudades(idDepartamento);
        return Ok(ciudades);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { mensaje = "Error al obtener los roles.", error = ex.Message });
      }
    }
  }
}
