using Org.BouncyCastle.Utilities.Net;
using System;
using System.Net;
using System.Net.Mail;

namespace ApiPersons.Utilities
{
    public class MailHelper
    {
        public static string MAIL_SEND = "dhdb4291@gmail.com";
        public static string SUBJECT = "Restablecimiento de contraseña";
        public static string bodyMail = @"
        <!DOCTYPE html>
        <html>
        <head>
            <title>Recuperación de Contraseña</title>
        </head>
        <body>
            <h1>Recuperación de contraseña</h1>
            <p>Hola <strong><em>{ username }</em></strong></p>
            <p>Recibes este correo porque has solicitado restablecer tu contraseña en nuestro sitio web.</p>
            <p>Por favor, haz clic en el siguiente enlace para restablecer tu contraseña:</p>
            <p><a href='{resetLink}'> Restablecer Contraseña </a></p>
            <p>Si no solicitaste este restablecimiento de contraseña, puedes ignorar este correo.</p>
            <p>Gracias,</p>
            <h3>TOKEN: { token } </h3>
            <hr>
            <p><strong><em>Equipo DreamStyle.</em></strong></p>
        </body>
        </html>
        ";

        public void SendEmail(string toAddress, string username, string token)
        {
            try
            {
                bodyMail = bodyMail.Replace("{ username }", username);
                bodyMail = bodyMail.Replace("{ token }", token);
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("dhdb4291@gmail.com", "soqz obnj ezsv lfmf"),
                    EnableSsl = true,
                };
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("noreply@example.com"),
                    Subject = SUBJECT,
                    Body = bodyMail,
                    IsBodyHtml = true, 
                };
                mailMessage.To.Add(toAddress);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }

        internal void SetSmtpClient(SmtpClient @object)
        {
            throw new NotImplementedException();
        }
    }
}
