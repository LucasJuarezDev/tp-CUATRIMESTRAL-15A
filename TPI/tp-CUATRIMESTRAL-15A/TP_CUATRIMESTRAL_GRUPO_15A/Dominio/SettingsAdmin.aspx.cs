using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class SettingsAdmin : AuthenticationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarConfiguracion();
            }
        }

        private void CargarConfiguracion()
        {
            var manager = new ConfigManager();

            // De CONFIGURACION
            txtWhatsApp.Text = manager.ObtenerString("WHATSAPP_ADMIN", "5491167152188");

            // De USUARIO (ADMIN)
            txtEmailAdmin.Text = manager.ObtenerEmailAdmin();
            if (string.IsNullOrEmpty(txtEmailAdmin.Text))
                txtEmailAdmin.Text = "admin@tutienda.com";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var manager = new ConfigManager();

                // GUARDAR EN CONFIGURACION
                manager.Guardar("WHATSAPP_ADMIN", txtWhatsApp.Text.Trim());

                // GUARDAR EMAIL EN TABLA USUARIO (solo admin)
                manager.ActualizarEmailAdmin(txtEmailAdmin.Text.Trim());

                MostrarExito("¡Todos los cambios se guardaron correctamente!");
            }
            catch (Exception ex)
            {
                MostrarError("Error al guardar: " + ex.Message);
            }
        }

        private void MostrarExito(string mensaje)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "exito",
                $"Swal.fire({{icon: 'success', title: '¡Perfecto!', text: '{mensaje}', timer: 3000}});", true);
        }

        private void MostrarError(string mensaje)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "error",
                $"Swal.fire({{icon: 'error', title: 'Error', text: '{mensaje}'}});", true);
        }
    }
}