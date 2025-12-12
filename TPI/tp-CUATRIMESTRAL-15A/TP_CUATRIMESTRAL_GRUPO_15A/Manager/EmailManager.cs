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

        public async Task EnviarMailCambioEstadoPago(string destino, string nombreCliente, long idVenta, string estadoNuevo)
        {
            string asunto = "";
            string cuerpo = "";

            if (estadoNuevo == "Aprobado")
            {
                asunto = "¡Tu pago fue aprobado!";
                cuerpo = $"¡Excelente noticia, {nombreCliente}!\n\n" +
                         $"Tu pago de la compra Nº *{idVenta}* fue *APROBADO*.\n\n" +
                         $"Ya estamos preparando tu pedido para enviártelo lo antes posible.\n\n" +
                         $"¡Gracias por confiar en nosotros!";
            }
            else if (estadoNuevo == "Rechazado")
            {
                asunto = "Tu pago fue rechazado";
                cuerpo = $"Hola {nombreCliente},\n\n" +
                         $"Lamentamos informarte que tu pago para la compra Nº *{idVenta}* fue *RECHAZADO*.\n\n" +
                         $"Por favor, intentá nuevamente o elegí otro método de pago.\n\n" +
                         $"Si creés que esto es un error, respondé este mail.";
            }
            else if (estadoNuevo == "Pendiente de comprobante")
            {
                asunto = "Esperando comprobante de transferencia";
                cuerpo = $"Hola {nombreCliente},\n\n" +
                         $"Recibimos tu pedido Nº *{idVenta}* por transferencia.\n\n" +
                         $"Por favor, enviá el comprobante lo antes posible para procesar tu compra.\n\n" +
                         $"¡Gracias por tu paciencia!";
            }
            else
            {
                asunto = "Actualización en tu pedido";
                cuerpo = $"Hola {nombreCliente},\n\n" +
                         $"Hubo una actualización en el estado de pago de tu compra Nº {idVenta}.\n" +
                         $"Estado actual: {estadoNuevo}\n\n" +
                         $"Saludos,\nEquipo Mi Tienda";
            }

            await EnviarMailPersonalizado(destino, nombreCliente, asunto, cuerpo);
        }

        public async Task EnviarMailCambioPreparacion(string destino, string nombreCliente, long idVenta, string estadoNuevo)
        {
            string asunto = "";
            string cuerpo = "";

            if (estadoNuevo == "En preparación")
            {
                asunto = "¡Tu pedido está siendo preparado!";
                cuerpo = $"¡Buenas noticias, {nombreCliente}!\n\n" +
                         $"Tu pedido Nº *{idVenta}* ya está *EN PREPARACIÓN*.\n\n" +
                         $"En breve estará listo para enviarte.\n\n" +
                         $"¡Gracias por tu paciencia!";
            }
            else if (estadoNuevo == "Listo para envío")
            {
                asunto = "¡Tu pedido ya está listo para enviar!";
                cuerpo = $"¡EXCELENTE, {nombreCliente}!\n\n" +
                         $"Tu pedido Nº *{idVenta}* ya está *LISTO PARA ENVÍO*.\n\n" +
                         $"En las próximas horas sale hacia tu domicilio.\n\n" +
                         $"¡Vas a recibirlo muy pronto!";
            }
            else if (estadoNuevo == "Rechazado")
            {
                asunto = "Tu pedido fue cancelado.";
                cuerpo = $"{nombreCliente}!\n\n" +
                         $"Tu pedido Nº *{idVenta} *fue Cancelado*.\n\n" +
                         $"En las próximas horas nos pondremos en contacto con vos por reembolso de dinero.\n\n" +
                         $"Muchas gracias por su paciencia.";
            }

            await EnviarMailPersonalizado(destino, nombreCliente, asunto, cuerpo);
        }

        public async Task EnviarMailCambioEnvio(string destino, string nombreCliente, long idVenta, string estadoNuevo)
        {
            string asunto = "";
            string cuerpo = "";

            if (estadoNuevo == "En camino")
            {
                asunto = "¡Tu pedido ya está en camino!";
                cuerpo = $"¡GRAN NOTICIA, {nombreCliente}!\n\n" +
                         $"Tu pedido Nº *{idVenta}* ya está *EN CAMINO* a tu domicilio.\n\n" +
                         $"¡En breve lo vas a tener en tus manos!";
            }
            else if (estadoNuevo == "Entregado")
            {
                asunto = "¡Tu pedido fue entregado!";
                cuerpo = $"¡Llegó tu pedido, {nombreCliente}!\n\n" +
                         $"La compra Nº *{idVenta}* fue *ENTREGADA* exitosamente.\n\n" +
                         $"¡Esperamos que disfrutes tu compra!\n" +
                         $"Si tenés algún comentario, respondé este mail";
            }
            else if (estadoNuevo == "Devuelto")
            {
                asunto = "Tu pedido fue devuelto";
                cuerpo = $"Hola {nombreCliente},\n\n" +
                         $"Tu pedido Nº *{idVenta}* fue *DEVUELTO*.\n\n" +
                         $"Esto puede deberse a que no se encontró nadie en el domicilio o dirección incorrecta.\n\n" +
                         $"Pronto nos pondremos en contacto para coordinar el reenvío o reembolso.\n\n" +
                         $"¡Gracias por tu comprensión!";
            }

            else if (estadoNuevo == "Cancelado")
            {
                asunto = "Tu envío fue cancelado";
                cuerpo = $"Hola {nombreCliente},\n\n" +
                         $"Lamentamos informarte que el envío de tu pedido Nº *{idVenta}* fue *CANCELADO*.\n\n" +
                         $"Si tenés dudas sobre el motivo, respondé este mail y te ayudamos.\n\n" +
                         $"Gracias por entender.";
            }

            await EnviarMailPersonalizado(destino, nombreCliente, asunto, cuerpo);
        }

        // MÉTODO GENÉRICO QUE USA TU CONFIGURACIÓN ACTUAL
        public async Task EnviarMailPersonalizado(string destino, string nombreCliente, string asunto, string cuerpo)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("axelpereyra965@gmail.com", "Mi Tienda Web");
                mail.To.Add(destino);
                mail.Subject = asunto;
                mail.Body = cuerpo;
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("axelpereyra965@gmail.com", "rcqofiqomnwacptm");

                await smtp.SendMailAsync(mail);

                // LOG PARA VER QUE SE ENVIÓ
                System.Diagnostics.Debug.WriteLine($"EMAIL ENVIADO A {destino} - Asunto: {asunto}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR AL ENVIAR EMAIL: " + ex.Message);
                throw; // Para que veas el error real
            }
        }
    }
}
