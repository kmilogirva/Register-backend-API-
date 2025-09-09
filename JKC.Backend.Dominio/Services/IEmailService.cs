namespace JKC.Backend.Dominio.Services
{
  public interface IEmailService
  {
    Task<bool> EnviarEmailAsync(IEnumerable<string> destinatarios, string asunto, string mensajeHtml);
  }
}
