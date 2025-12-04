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
            txtCostoEnvio.Text = manager.ObtenerDecimal("COSTO_ENVIO", 2500).ToString();
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
                manager.Guardar("COSTO_ENVIO", txtCostoEnvio.Text.Trim());
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

        protected void btnProbarWhatsApp_Click(object sender, EventArgs e)
        {
            string numero = txtWhatsApp.Text.Trim();
            if (string.IsNullOrWhiteSpace(numero))
            {
                MostrarError("Ingresa un número de WhatsApp primero");
                return;
            }

            string url = $"https://api.whatsapp.com/send/?phone={numero}&text=¡Hola!%20Este%20es%20un%20mensaje%20de%20prueba%20desde%20tu%20tienda%20🛒&type=phone_number&app_absent=0";
            ClientScript.RegisterStartupScript(this.GetType(), "probar", $"window.open('{url}','_blank');", true);
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