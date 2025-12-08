using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class EmailManager
    {
        public async Task EnviarMailVenta(string destino, string nombreCliente, long idVenta, decimal total)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("axelpereyra965@gmail.com", "Mi Tienda Web");
            mail.To.Add(destino);

            mail.Subject = "¡Gracias por tu compra!";
            mail.Body =
                $"Hola {nombreCliente},\n\n" +
                $"¡Gracias por tu compra!\n" +
                $"Número de operación: {idVenta}\n" +
                $"Total: ${total:N0}\n\n" +
                $"Pronto nos pondremos en contacto para coordinar el envío.\n\n" +
                $"Saludos,\n" +
                $"Equipo MiTienda 😉";

            mail.IsBodyHtml = false;

            // 🔹 Configuracion para Gmail 
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            smtp.Credentials = new NetworkCredential(
                "axelpereyra965@gmail.com",
                "rcqofiqomnwacptm"
            );

            await smtp.SendMailAsync(mail);
        }
    }
}
