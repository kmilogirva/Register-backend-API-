using System;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Jkc.Backend.Aplicacion.Services.EmailService
{
  public class EmailService : IEmailService
  {

    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
      _settings = options.Value;
    }

    public async Task<bool> EnviarEmailAsync(string destinatario, string asunto, string mensajeHtml)
    {
      try
      {
        using var cliente = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
          Credentials = new NetworkCredential(_settings.Email, _settings.Password),
          EnableSsl = true
        };

        var mail = new MailMessage
        {
          From = new MailAddress(_settings.Email),
          Subject = asunto,
          Body = mensajeHtml,
          IsBodyHtml = true
        };
        mail.To.Add(destinatario);

        await cliente.SendMailAsync(mail);
        return true;
      }
      catch
      {
        return false;
      }
    }

  }
}

