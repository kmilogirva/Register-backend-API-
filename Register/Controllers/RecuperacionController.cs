using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.WebApi.Models; // DTOs (ver archivos abajo)
using JKC.Backend.Dominio.Entidades.Usuario; // si necesitas el tipo Usuario (opcional)

namespace JKC.Backend.WebApi.Controllers
{
  [ApiController]
  [Route("api/recuperacion")]
  public class RecuperacionController : ControllerBase
  {
    private readonly IServicioUsuario _servicioUsuario;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;

    public RecuperacionController(
        IServicioUsuario servicioUsuario,
        IConfiguration config,
        IServiceProvider serviceProvider)
    {
      _servicioUsuario = servicioUsuario ?? throw new ArgumentNullException(nameof(servicioUsuario));
      _config = config ?? throw new ArgumentNullException(nameof(config));
      _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Solicitar recuperación: genera token, lo guarda en BD y (si SMTP configurado) envía correo.
    /// </summary>
    [HttpPost("solicitar")]
    public async Task<IActionResult> Solicitar([FromBody] SolicitarRecuperacionDto dto)
    {
      if (dto == null || string.IsNullOrWhiteSpace(dto.Correo))
        return BadRequest(new { mensaje = "Correo requerido." });

      // Genera token y guarda en BD (servicio)
      var resultado = await _servicioUsuario.SolicitarRecuperacionContrasenaAsync(dto.Correo);
      if (!resultado.Exitoso)
        return BadRequest(new { mensaje = resultado.Mensaje });

      // Obtener usuario para recoger token guardado (no devolvemos token directamente salvo en modo prueba)
      var usuario = await _servicioUsuario.ObtenerUsuarioPorCorreo(dto.Correo);
      var token = usuario?.TokenRecuperacion;

      // Intentamos enviar correo si hay servicio y configuración SMTP
      var smtpHost = _config["Smtp:Host"];
      var emailService = _serviceProvider.GetService(typeof(IEmailService)) as IEmailService;

      if (!string.IsNullOrEmpty(smtpHost) && emailService != null && !string.IsNullOrEmpty(token))
      {
        // Construir link al frontend
        var frontendUrl = _config["Frontend:Url"] ?? "http://localhost:4200";
        var link = $"{frontendUrl.TrimEnd('/')}/restablecer?token={token}";
        var body = $"Haz clic en el siguiente enlace para restablecer tu contraseña:\n\n{link}\n\nEl enlace expira en 1 hora.";

        try
        {
          await emailService.SendEmailAsync(usuario.Correo, "Recuperación de contraseña", body);
          return Ok(new { mensaje = "Se ha enviado un correo con instrucciones." });
        }
        catch (Exception ex)
        {
          // Si falla enviar correo, devolvemos token en modo prueba para no bloquear desarrollo
          return Ok(new { mensaje = "No se pudo enviar el correo. Token generado (modo prueba).", token });
        }
      }

      // Si no hay SMTP o no hay emailService, devolvemos token (modo prueba). NO usar en producción.
      return Ok(new { mensaje = "Token generado (modo prueba). Si tienes SMTP configurado, el correo será enviado.", token });
    }

    /// <summary>
    /// Restablecer contraseña usando token.
    /// </summary>
    [HttpPost("restablecer")]
    public async Task<IActionResult> Restablecer([FromBody] RestablecerContrasenaDto dto)
    {
      if (dto == null || string.IsNullOrWhiteSpace(dto.Token) || string.IsNullOrWhiteSpace(dto.NuevaContrasena))
        return BadRequest(new { mensaje = "Token y nueva contraseña son requeridos." });

      var resultado = await _servicioUsuario.RestablecerContrasenaAsync(dto.Token, dto.NuevaContrasena);
      if (!resultado.Exitoso)
        return BadRequest(new { mensaje = resultado.Mensaje });

      return Ok(new { mensaje = resultado.Mensaje });
    }
  }

  // Nota: la interfaz IEmailService no está en tu solución por defecto.
  // Si ya la creaste (SmtpEmailService), el controller la consumirá vía IServiceProvider (opcional).
  public interface IEmailService
  {
    Task SendEmailAsync(string to, string subject, string body);
  }
}
