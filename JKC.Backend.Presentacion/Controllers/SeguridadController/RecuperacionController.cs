using JKC.Backend.Aplicacion.Services.UsuarioServices;
using JKC.Backend.Dominio.Entidades.Seguridad.RecuperacionClave;
using JKC.Backend.Dominio.Services;
using Microsoft.AspNetCore.Mvc;

namespace JKC.Backend.WebApi.Controllers.SeguridadController
{

  [ApiController]
  [Route("api/[controller]")]
  public class RecuperacionController : ControllerBase
  {

    private readonly IServicioUsuario _servicioUsuario;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEmailService _emailService;

    public RecuperacionController(
        IServicioUsuario servicioUsuario,
        IConfiguration config,
        IServiceProvider serviceProvider,
         IEmailService emailService)
    {
      _servicioUsuario = servicioUsuario ?? throw new ArgumentNullException(nameof(servicioUsuario));
      _config = config ?? throw new ArgumentNullException(nameof(config));
      _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
      _emailService = emailService;
    }

    [HttpPost("solicitar-recuperacion/{correo}")]
    public async Task<IActionResult> Solicitar(string correo)
    {
      if (string.IsNullOrWhiteSpace(correo))
        return BadRequest(new { mensaje = "Correo requerido." });

      var (respuesta, token) = await _servicioUsuario.SolicitarRecuperacionContrasenaAsync(correo);

      if (!respuesta.Exitoso)
        return BadRequest(new { mensaje = respuesta.Mensaje });

      var frontendUrl = _config["Frontend:Url"] ?? "http://localhost:4200";
      var link = $"{frontendUrl.TrimEnd('/')}/resetear-contrasena/{token}";
      //COMO MANDAR ESTO AL FRONT
      // Plantilla HTML similar a la de "test"
      string mensajeHtml = $@"
                <!DOCTYPE html>
                <html>
                <head>
                <meta charset='UTF-8'>
                <title>Recuperación de Contraseña</title>
                </head>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
                <table width='100%' cellpadding='0' cellspacing='0' style='background-color: #f4f4f4; padding: 20px 0;'>
                <tr>
                <td align='center'>
                  <table width='600' cellpadding='0' cellspacing='0' style='background: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                    <!-- Encabezado -->
                    <tr>
                      <td align='center' style='background-color: #2C3E50; padding: 20px;'>
                        <h1 style='color: #ffffff; margin: 0; font-size: 24px;'>JKC Inventory</h1>
                      </td>
                    </tr>
                    <!-- Contenido -->
                    <tr>
                      <td style='padding: 30px; color: #333333; font-size: 16px; line-height: 1.5;'>
                        <p>Hola,</p>
                        <p>Has solicitado restablecer tu contraseña.</p>
                        <p>Haz clic en el siguiente enlace para continuar:</p>
                        <p style='margin-top: 20px;'>
                          <a href='{link}' style='display: inline-block; background-color: #27ae60; color: #ffffff; padding: 10px 20px; border-radius: 5px; text-decoration: none;'>Restablecer contraseña</a>
                        </p>
                        <p>Si no solicitaste este cambio, ignora este mensaje.</p>
                        <p><small>Este enlace expirará en 1 hora.</small></p>
                      </td>
                    </tr>
                    <!-- Footer -->
                    <tr>
                      <td align='center' style='background-color: #ecf0f1; padding: 15px; font-size: 12px; color: #7f8c8d;'>
                        © 2025 JKC Inventory. Todos los derechos reservados.
                      </td>
                    </tr>
                  </table>
                </td>
                </tr>
                </table>
                </body>
                </html>";

      // Usamos el mismo servicio que en tu método de prueba
      bool enviado = await _emailService.EnviarEmailAsync(
          new[] { correo },
          "Recuperación de contraseña - JKC Inventory",
          mensajeHtml
      );

      if (enviado)
      {
        return Ok(new { mensaje = "Se ha enviado un correo con instrucciones." });
      }
      else
      {
        return Ok(new { mensaje = "No se pudo enviar el correo. Token generado (modo prueba).", token });
      }
    }



    /// <summary>
    /// Restablecer contraseña usando token.
    /// </summary>
    [HttpPost("restablecer-contrasena")]
    public async Task<IActionResult> Restablecer([FromBody] RestablecerContrasenaDto dto)
    {
      if (dto == null || string.IsNullOrWhiteSpace(dto.Token) || string.IsNullOrWhiteSpace(dto.NuevaContrasena))
        return BadRequest(new { mensaje = "Token y nueva contraseña son requeridos." });

      var resultado = await _servicioUsuario.RestablecerContrasenaAsync(dto.Token, dto.NuevaContrasena);
      if (!resultado.Exitoso)
        return BadRequest(new { mensaje = resultado.Mensaje });

      return Ok(new { mensaje = resultado.Mensaje });
    }



    [HttpGet("validar-token/{token}")]
    public async Task<IActionResult> ValidarToken(string token)
    {
      bool esValido = await _servicioUsuario.ValidarTokenAsync(token);
      return Ok(new { valido = esValido });
    }

  }
}

