using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;

namespace Dominio
{
    public partial class RegistrarUsuario1 : System.Web.UI.Page
    {
        private UsuarioManager usuarioManager = new UsuarioManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarRoles();  // Tu método para ddlRol
            }
            else
            {
                // Detecta parámetro de éxito (solo en postback)
                if (Request.QueryString["exito"] == "true")
                {
                    // Muestra SweetAlert con JavaScript
                    string script = @"<script type='text/javascript'>
                                        mostrarAlertaExito();
                                      </script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertaExito", script, false);
                }
            }
        }

        private void CargarRoles()
        {
            ddlRol.Items.Clear();
            ddlRol.Items.Add(new System.Web.UI.WebControls.ListItem("-- Seleccionar rol --", ""));
            ddlRol.Items.Add(new System.Web.UI.WebControls.ListItem("Empleado", "2"));
            ddlRol.Items.Add(new System.Web.UI.WebControls.ListItem("Cliente", "3"));
        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                // Validar contraseñas
                if (txtPassword.Text != txtRepetirPassword.Text)
                {
                    MostrarMensaje("Las contraseñas no coinciden.", "alert-danger");
                    return;
                }

                // Crear objeto Rol
                Rol rol = new Rol
                {
                    Id = (byte)Convert.ToInt64(ddlRol.SelectedValue),
                    Nombre = ddlRol.SelectedItem.Text
                };

                // Crear usuario
                Usuario nuevoUsuario = new Usuario
                {
                    Nickname = txtNickname.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Contrasena = txtPassword.Text, 
                    Rol = rol,
                    Activo = true
                };

                // Llamar al Manager
                long nuevoId = usuarioManager.Agregar(nuevoUsuario);

                if (nuevoId > 0)
                {
                    MostrarMensaje($"Usuario creado con éxito. ID: {nuevoId}", "alert-success");
                    LimpiarFormulario();
                }
                else
                {
                    MostrarMensaje("Error al crear usuario.", "alert-danger");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, "alert-warning");
            }
        }

        private void MostrarMensaje(string texto, string tipo)
        {
            lblMensaje.Text = texto;
            lblMensaje.CssClass = $"alert {tipo} d-block text-center";
            lblMensaje.Visible = true;
        }

        private void LimpiarFormulario()
        {
            txtNickname.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtRepetirPassword.Text = "";
            ddlRol.SelectedIndex = 0;
        }
    }
}