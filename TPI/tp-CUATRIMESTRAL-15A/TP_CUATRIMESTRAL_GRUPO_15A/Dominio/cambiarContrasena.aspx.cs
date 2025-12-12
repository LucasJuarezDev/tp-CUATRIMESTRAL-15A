using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class cambiarContrasena : System.Web.UI.Page
    {
        private readonly UsuarioManager usuarioManager = new UsuarioManager();
        private readonly EmailManager emailManager = new EmailManager();
        private string CodigoGenerado { get; set; }
        private string EmailTemporal { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divEmail.Visible = true;
                divCodigo.Visible = false;
                divExito.Visible = false;
            }
        }

        protected async void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                MostrarError("Ingresa tu email");
                return;
            }

            var usuario = usuarioManager.ObtenerPorEmail(email);
            if (usuario == null || !usuario.Activo)
            {
                MostrarError("No existe una cuenta activa con ese email");
                return;
            }

            Random rnd = new Random();
            CodigoGenerado = rnd.Next(100000, 999999).ToString();

            Session["CodigoVerificacion"] = CodigoGenerado;
            Session["EmailVerificacion"] = email;

            try
            {
                await emailManager.EnviarMailPersonalizado(
                    email,
                    usuario.Nickname,
                    "Código de verificación - Mi Tienda",
                    $"Hola {usuario.Nickname},\n\n" +
                    $"Tu código de verificación es:\n\n" +
                    $"     {CodigoGenerado}\n\n" +
                    $"Este código expira en 10 minutos.\n\n" +
                    $"Si no solicitaste este cambio, ignorá este mensaje.\n\n" +
                    $"Equipo Mi Tienda"
                );

                divEmail.Visible = false;
                divCodigo.Visible = true;
                lblEmailEnviado.Text = email;
            }
            catch (Exception ex)
            {
                MostrarError("Error al enviar el código. Intentá de nuevo.");
            }
        }

        protected void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            if (Session["CodigoVerificacion"] == null || Session["EmailVerificacion"] == null)
            {
                MostrarError("Sesión expirada. Volvé a solicitar el código.");
                VolverAPaso1();
                return;
            }

            string codigoIngresado = txtCodigo.Text.Trim();
            string codigoGuardado = Session["CodigoVerificacion"].ToString();

            if (codigoIngresado != codigoGuardado)
            {
                MostrarError("Código incorrecto");
                return;
            }

            if (txtNuevaPassword.Text.Length < 6)
            {
                MostrarError("La contraseña debe tener al menos 6 caracteres");
                return;
            }

            if (txtNuevaPassword.Text != txtRepetirPassword.Text)
            {
                MostrarError("Las contraseñas no coinciden");
                return;
            }

            try
            {
                string email = Session["EmailVerificacion"].ToString();
                var usuario = usuarioManager.ObtenerPorEmail(email);
                if (usuario != null)
                {
                    usuarioManager.ActualizarPassword(usuario.Id, txtNuevaPassword.Text);

                    // Limpiar sesión
                    Session.Remove("CodigoVerificacion");
                    Session.Remove("EmailVerificacion");

                    divCodigo.Visible = false;
                    divExito.Visible = true;
                }
            }
            catch (Exception)
            {
                MostrarError("Error al cambiar la contraseña");
            }
        }

        protected void lnkVolverEmail_Click(object sender, EventArgs e)
        {
            VolverAPaso1();
        }

        private void VolverAPaso1()
        {
            Session.Remove("CodigoVerificacion");
            Session.Remove("EmailVerificacion");
            divEmail.Visible = true;
            divCodigo.Visible = false;
            divExito.Visible = false;
            txtEmail.Text = "";
        }

        private void MostrarError(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                $"Swal.fire({{icon: 'error', title: 'Error', text: '{mensaje}'}});", true);
        }
    }
}